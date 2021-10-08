using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        private readonly IPasswordHasher<User> passwordHasher;

        public UserService(IUserRepository userRepository, 
            ITutorRepository tutorRepository, 
            IStudentRepository studentRepository, 
            IMapper mapper, 
            IPasswordHasher<User> passwordHasher)
        {
            this.userRepository = userRepository;
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
            this.passwordHasher = passwordHasher;
        }

        public async Task<UserDto> TryLoginAsync(LoginUserDto userModel)
        {
            var passwordVerificationResult = await ValidatePasswordAsync(userModel);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                return null;

            var user = await userRepository.GetUserByUsernameAsync(userModel.Username);
            if (user != null)
                await SetLastLoginDate(user);

            return mapper.Map<UserDto>(user);
        }

        public async Task<Role> GetUserRoleAsync(long userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            return user.Role;
        }

        public async Task<bool> RegisterStudentAsync(RegisterStudentDto student)
        {
            var newStudent = mapper.Map<Student>(student);
            newStudent.PasswordHash = passwordHasher.HashPassword(newStudent, student.Password);

            return await studentRepository.AddStudentAsycn(newStudent);
        }

        public async Task<bool> RegisterTutorAsync(RegisterTutorDto tutor)
        {
            var newTutor = mapper.Map<Tutor>(tutor);
            newTutor.PasswordHash = passwordHasher.HashPassword(newTutor, tutor.Password);

            return await tutorRepository.AddTutorAsync(newTutor);
        }

        public async Task<bool> DeactivateUserAsync(long userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);

            return await userRepository.DeleteUserAsync(user);
        }

        public async Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(long userId, PasswordDto passwordModel)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            var validationResult = ValidatePassword(user, passwordModel);

            if (validationResult.Count == 0)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, passwordModel.NewPassword);
                var changed = await userRepository.UpdateUser(user);

                if (!changed)
                {
                    validationResult.Add(WrongPasswordStatus.DatabaseError);
                    return validationResult;
                }

                return null;
            }

            return validationResult;
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
                result.Add(WrongPasswordStatus.PasswordsVary);

            if (passwordModel.NewPassword.Length < 4)
                result.Add(WrongPasswordStatus.TooShort);

            passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordModel.NewPassword);
            if (passwordVerificationResult.Equals(PasswordVerificationResult.Success))
                result.Add(WrongPasswordStatus.DuplicateOfOld);

            return result;
        }

        private async Task<PasswordVerificationResult> ValidatePasswordAsync(LoginUserDto loginModel)
        {
            var user = await userRepository.GetUserByUsernameAsync(loginModel.Username);
            if (user is null)
                return PasswordVerificationResult.Failed;

            return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);
        }

        private async Task SetLastLoginDate(User user)
        {
            user.LastLoginDate = DateTime.Now;

            await userRepository.UpdateUser(user);
        }
    }
}
