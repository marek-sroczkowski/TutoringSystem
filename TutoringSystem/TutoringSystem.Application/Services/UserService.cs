using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.EmailDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IEmailService emailService;
        private readonly IActivationTokenService activationTokenService;
        private readonly IMapper mapper;
        private readonly IPasswordHasher<User> passwordHasher;

        public UserService(IUserRepository userRepository,
            ITutorRepository tutorRepository,
            IStudentRepository studentRepository,
            IEmailService emailService,
            IActivationTokenService activationTokenService,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
        {
            this.userRepository = userRepository;
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
            this.emailService = emailService;
            this.activationTokenService = activationTokenService;
            this.mapper = mapper;
            this.passwordHasher = passwordHasher;
        }

        public async Task<LoginResposneDto> TryLoginAsync(LoginUserDto userModel)
        {
            await DeactivateNotEnabledUsersAsync();

            var user = await userRepository.GetUserAsync(u => u.Username.Equals(userModel.Username), isEagerLoadingEnabled: true);
            if (user is null || ValidatePassword(userModel, user) == PasswordVerificationResult.Failed)
            {
                return new LoginResposneDto(LoginStatus.InvalidUsernameOrPassword, null);
            }

            await SetLastLoginDateAsync(user);

            return !user.IsEnable
                ? new LoginResposneDto(LoginStatus.InactiveAccount, mapper.Map<UserDto>(user))
                : new LoginResposneDto(LoginStatus.LoggedInCorrectly, mapper.Map<UserDto>(user));
        }

        public async Task<bool> RegisterStudentAsync(long tutorId, RegisterStudentDto student)
        {
            await DeactivateNotEnabledUsersAsync();

            var newStudent = mapper.Map<Student>(student);
            newStudent.PasswordHash = passwordHasher.HashPassword(newStudent, student.Password);
            if (!await studentRepository.AddStudentAsync(newStudent))
            {
                return false;
            }

            newStudent.StudentTutors = new List<StudentTutor> { new StudentTutor(newStudent.Id, tutorId, student.HourlRate, student.Note) };

            return await studentRepository.UpdateStudentAsync(newStudent);
        }

        public async Task<TutorDto> RegisterTutorAsync(RegisterTutorDto newTutor)
        {
            await DeactivateNotEnabledUsersAsync();

            var tutor = mapper.Map<Tutor>(newTutor);
            tutor.PasswordHash = passwordHasher.HashPassword(tutor, newTutor.Password);

            var created = await tutorRepository.AddTutorAsync(tutor);

            return created ? mapper.Map<TutorDto>(tutor) : null;
        }

        public async Task<bool> ActivateUserByTokenAsync(long userId, string token)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId) && !u.IsEnable, isEagerLoadingEnabled: true);
            if (user is null)
            {
                return false;
            }

            var userToken = user.ActivationTokens.FirstOrDefault(t => t.ExpirationDate > DateTime.Now && t.TokenContent.Equals(token));
            if (userToken is null)
            {
                return false;
            }

            user.IsEnable = true;
            userToken.ExpirationDate = DateTime.Now;

            return await userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> SendNewActivationTokenAsync(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId) && !u.IsEnable, isEagerLoadingEnabled: true);
            if (user is null)
            {
                return false;
            }

            var generatedToken = await activationTokenService.AddActivationTokenAsync(userId);
            if (generatedToken is null || string.IsNullOrEmpty(generatedToken.TokenContent))
            {
                return false;
            }

            emailService.SendEmail(new ActivationEmailDto(user.Contact.Email, user.FirstName, generatedToken.TokenContent));

            return true;
        }

        public async Task<bool> DeactivateUserAsync(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId), isEagerLoadingEnabled: true);

            return await userRepository.RemoveUserAsync(user);
        }

        public async Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(long userId, PasswordDto passwordModel)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId), isEagerLoadingEnabled: true);
            var validationResult = ValidatePassword(user, passwordModel);

            if (validationResult.Count == 0)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, passwordModel.NewPassword);
                var changed = await userRepository.UpdateUserAsync(user);

                if (!changed)
                {
                    validationResult.Add(WrongPasswordStatus.InternalError);
                    return validationResult;
                }

                return null;
            }

            return validationResult;
        }

        public async Task<bool> UpdateGeneralUserInfoAsync(long userId, UpdatedUserDto updatedUser)
        {
            var existingUser = await userRepository.GetUserAsync(u => u.Id.Equals(userId), isEagerLoadingEnabled: true);
            var user = mapper.Map(updatedUser, existingUser);

            return await userRepository.UpdateUserAsync(user);
        }

        public async Task<ShortUserDto> GetGeneralUserInfoAsync(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));

            return mapper.Map<ShortUserDto>(user);
        }

        private ICollection<WrongPasswordStatus> ValidatePassword(User user, PasswordDto passwordModel)
        {
            var result = new List<WrongPasswordStatus>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordModel.OldPassword);

            if (!passwordVerificationResult.Equals(PasswordVerificationResult.Success))
            {
                result.Add(WrongPasswordStatus.InvalidOldPassword);
                return result;
            }

            if (!passwordModel.NewPassword.Equals(passwordModel.ConfirmPassword))
            {
                result.Add(WrongPasswordStatus.PasswordsVary);
            }

            if (passwordModel.NewPassword.Length < 4)
            {
                result.Add(WrongPasswordStatus.TooShort);
            }

            passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordModel.NewPassword);
            if (passwordVerificationResult.Equals(PasswordVerificationResult.Success))
            {
                result.Add(WrongPasswordStatus.DuplicateOfOld);
            }

            return result;
        }

        private PasswordVerificationResult ValidatePassword(LoginUserDto loginModel, User user)
        {
            return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);
        }

        private async Task SetLastLoginDateAsync(User user)
        {
            user.LastLoginDate = DateTime.Now;
            await userRepository.UpdateUserAsync(user);
        }

        private async Task DeactivateNotEnabledUsersAsync()
        {
            var tutors = await tutorRepository.GetTutorsCollectionAsync(u => !u.IsEnable && u.IsActive && u.RegistrationDate.AddDays(1) < DateTime.Now, isEagerLoadingEnabled: true);
            tutors.ToList().ForEach(u => u.IsActive = false);

            await tutorRepository.UpdateTutorsCollectionAsync(tutors);
        }
    }
}