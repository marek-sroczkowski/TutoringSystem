﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(LoginUserDto userModel);
        Task<bool> RegisterStudentAsync(RegisterStudentDto student);
        Task<bool> RegisterTutorAsync(RegisterTutorDto tutor);
        Task<PasswordVerificationResult> ValidatePasswordAsync(LoginUserDto loginModel);
        Task<Role> GetUserRoleAsync(long userId);
        Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(long userId, PasswordDto passwordModel);
    }
}
