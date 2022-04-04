using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.EmailDtos;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Extensions;
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

            emailService.SendEmail(new ActivationEmailDto(user.Contact.Email, user.FirstName, generatedToken.TokenContent));

            return true;
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
    }
}