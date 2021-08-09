using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Service.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Service
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

        public async Task<UserDto> GetUser(LoginUserDto userModel)
        {
            var user = await userRepository.GetUserByUsernameAsync(userModel.Username);

            return mapper.Map<UserDto>(user);
        }

        public async Task<Role> GetUserRole(long userId)
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

        public async Task<PasswordVerificationResult> ValidatePassword(LoginUserDto loginModel)
        {
            var user = await userRepository.GetUserByUsernameAsync(loginModel.Username);

            return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);
        }
    }
}
