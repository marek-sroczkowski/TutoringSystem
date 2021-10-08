using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> TryLoginAsync(LoginUserDto userModel);
        Task<bool> RegisterStudentAsync(RegisterStudentDto student);
        Task<bool> RegisterTutorAsync(RegisterTutorDto tutor);
        Task<bool> DeactivateUserAsync(long userId);
        Task<Role> GetUserRoleAsync(long userId);
        Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(long userId, PasswordDto passwordModel);
    }
}
