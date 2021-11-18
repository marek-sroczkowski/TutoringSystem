using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoginResultDto> TryLoginAsync(LoginUserDto userModel);
        Task<bool> RegisterStudentAsync(long tutorId, RegisterStudentDto student);
        Task<bool> RegisterTutorAsync(RegisterTutorDto tutor);
        Task<bool> DeactivateUserAsync(long userId);
        Task<ICollection<WrongPasswordStatus>> ChangePasswordAsync(long userId, PasswordDto passwordModel);
        Task<bool> ActivateUserByTokenAsync(long userId, string token);
        Task<bool> SendNewActivationTokenAsync(long userId);
        Task<bool> UpdateGeneralUserInfoAsync(long userId, UpdatedUserDto updatedUser);
        Task<ShortUserDto> GetGeneralUserInfoAsync(long userId);
    }
}
