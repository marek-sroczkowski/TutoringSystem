﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.TutorDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResposneDto> TryLoginAsync(LoginUserDto userModel);
        Task<bool> CreateNewStudentAsync(long tutorId, NewStudentDto student);
        Task<TutorDto> RegisterTutorAsync(RegisteredTutorDto tutor);
        Task<bool> DeactivateUserAsync(long userId);
        Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(long userId, PasswordDto passwordModel);
        Task<bool> ActivateUserByTokenAsync(long userId, string token);
        Task<bool> SendNewActivationTokenAsync(long userId);
        Task<bool> UpdateGeneralUserInfoAsync(long userId, UpdatedUserDto updatedUser);
        Task<ShortUserDto> GetGeneralUserInfoAsync(long userId);
        Task<StudentDto> RegisterStudentAsync(long userId, RegisteredStudentDto student);
    }
}