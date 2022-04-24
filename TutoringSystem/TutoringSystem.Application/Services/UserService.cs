using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Account;
using TutoringSystem.Application.Dtos.EmailDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Helpers;
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
        private readonly IContactRepository contactRepository;
        private readonly IPasswordResetCodeRepository passwordResetCodeRepository;
        private readonly IEmailService emailService;
        private readonly IActivationTokenService activationTokenService;
        private readonly IMapper mapper;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AppSettings settings;

        public UserService(IUserRepository userRepository,
            ITutorRepository tutorRepository,
            IStudentRepository studentRepository,
            IContactRepository contactRepository,
            IPasswordResetCodeRepository passwordResetCodeRepository,
            IEmailService emailService,
            IActivationTokenService activationTokenService,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher,
            IOptions<AppSettings> settings)
        {
            this.userRepository = userRepository;
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
            this.contactRepository = contactRepository;
            this.passwordResetCodeRepository = passwordResetCodeRepository;
            this.emailService = emailService;
            this.activationTokenService = activationTokenService;
            this.mapper = mapper;
            this.passwordHasher = passwordHasher;
            this.settings = settings.Value;
        }

        public async Task<bool> CreateNewStudentAsync(long tutorId, NewStudentDto student)
        {
            var newStudent = mapper.Map<Student>(student);
            if (!await studentRepository.AddStudentAsync(newStudent))
            {
                return false;
            }

            newStudent.StudentTutors = new List<StudentTutor> { new StudentTutor(newStudent.Id, tutorId, student.HourlRate, student.Note) };

            return await studentRepository.UpdateStudentAsync(newStudent);
        }

        public async Task<StudentDto> RegisterStudentAsync(long userId, RegisteredStudentDto student)
        {
            var existingStudent = await studentRepository.GetStudentAsync(s => s.Id.Equals(userId), isEagerLoadingEnabled: true);
            existingStudent.PasswordHash = passwordHasher.HashPassword(existingStudent, student.Password);
            existingStudent.Contact.Email = student.Email;
            var updated = await studentRepository.UpdateStudentAsync(existingStudent);

            return updated ? mapper.Map<StudentDto>(existingStudent) : null;
        }

        public async Task<TutorDto> RegisterTutorAsync(RegisteredTutorDto newTutor)
        {
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

            var now = DateTime.Now.ToLocal();
            var userToken = user.ActivationTokens.FirstOrDefault(t => t.ExpirationDate > now && t.TokenContent.Equals(token));
            if (userToken is null)
            {
                return false;
            }

            user.IsEnable = true;
            userToken.ExpirationDate = DateTime.Now.ToLocal();

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

            return await emailService.SendActivationCodeAsync(new ActivationEmailDto(user.Contact.Email, user.FirstName, generatedToken.TokenContent));
        }

        public async Task<bool> DeactivateUserAsync(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId), isEagerLoadingEnabled: true);

            return await userRepository.RemoveUserAsync(user);
        }

        public async Task<bool> ChangePasswordAsync(long userId, PasswordDto passwordModel)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId), isEagerLoadingEnabled: true);
            user.PasswordHash = passwordHasher.HashPassword(user, passwordModel.NewPassword);

            return await userRepository.UpdateUserAsync(user);
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

        public async Task<PasswordResetCodeSendingResultDto> SendPasswordResetCodeAsync(string email)
        {
            var contact = await contactRepository.GetContactAsync(c => c.Email == email);
            if (contact is null)
            {
                return new PasswordResetCodeSendingResultDto(PasswordResetCodeSendingResult.InvalidEmail);
            }

            await DeactivateOldCodeAsync(contact.UserId);
            var code = GetPasswordResetCode(contact.UserId, email);
            await passwordResetCodeRepository.AddCodeAsync(code);
            var passResetEmail = new PasswordResetEmailDto(email, code.Code);
            var sent = await emailService.SendPasswordResetCodeAsync(passResetEmail);

            return sent
                ? new PasswordResetCodeSendingResultDto(PasswordResetCodeSendingResult.Success)
                : new PasswordResetCodeSendingResultDto(PasswordResetCodeSendingResult.InternalError);
        }

        public async Task<PasswordResetCodeValidationResultDto> ValidatePasswordResetCodeAsync(PasswordResetCodeDto resetCode)
        {
            var contact = await contactRepository.GetContactAsync(c => c.Email == resetCode.Email);
            if (contact is null)
            {
                return new PasswordResetCodeValidationResultDto(PasswordResetCodeValidationResult.IncorrectCode);
            }

            var code = await passwordResetCodeRepository.GetCodeAsync(c => c.Code == resetCode.Code && c.Email == resetCode.Email && c.UserId == contact.UserId && c.IsActive);
            if (code is null)
            {
                return new PasswordResetCodeValidationResultDto(PasswordResetCodeValidationResult.IncorrectCode);
            }

            return code.ExpirationDate < DateTime.Now.ToLocal()
                ? new PasswordResetCodeValidationResultDto(PasswordResetCodeValidationResult.ExpiredToken)
                : new PasswordResetCodeValidationResultDto(PasswordResetCodeValidationResult.Success);
        }

        public async Task<bool> ResetPasswordAsync(NewPasswordDto newPassword)
        {
            var contact = await contactRepository.GetContactAsync(c => c.Email == newPassword.Email);
            var user = await userRepository.GetUserAsync(u => u.Id == contact.UserId);
            user.PasswordHash = passwordHasher.HashPassword(user, newPassword.Password);

            return await userRepository.UpdateUserAsync(user);
        }

        private async Task DeactivateOldCodeAsync(long userId)
        {
            var code = await passwordResetCodeRepository.GetCodeAsync(c => c.UserId == userId && c.IsActive);
            if (code is null)
            {
                return;
            }

            code.ExpirationDate = DateTime.Now.ToLocal();
            await passwordResetCodeRepository.RemoveCodeAsync(code);
        }

        private PasswordResetCode GetPasswordResetCode(long userId, string email)
        {
            return new PasswordResetCode
            {
                Code = new Random().Next(10000, 100000).ToString(),
                Email = email,
                UserId = userId,
                ExpirationDate = DateTime.Now.ToLocal().AddDays(settings.PasswordResetCodeExpireDays)
            };
        }
    }
}