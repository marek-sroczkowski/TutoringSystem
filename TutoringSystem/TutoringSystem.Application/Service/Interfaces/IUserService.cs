using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Service.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUser(LoginUserDto userModel);
        Task<bool> RegisterStudentAsync(RegisterStudentDto student);
        Task<bool> RegisterTutorAsync(RegisterTutorDto tutor);
        Task<PasswordVerificationResult> ValidatePassword(LoginUserDto loginModel);
        Task<Role> GetUserRole(long userId);
    }
}
