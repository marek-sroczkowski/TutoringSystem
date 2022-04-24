using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Application.Models.Dtos.Password;
using TutoringSystem.Application.Models.Dtos.Student;
using TutoringSystem.Application.Models.Dtos.Tutor;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateNewStudentAsync(long tutorId, NewStudentDto student);
        Task<TutorDto> RegisterTutorAsync(RegisteredTutorDto tutor);
        Task<bool> DeactivateUserAsync(long userId);
        Task<bool> ChangePasswordAsync(long userId, PasswordDto passwordModel);
        Task<bool> ActivateUserByTokenAsync(long userId, string token);
        Task<bool> SendNewActivationTokenAsync(long userId);
        Task<bool> UpdateGeneralUserInfoAsync(long userId, UpdatedUserDto updatedUser);
        Task<ShortUserDto> GetGeneralUserInfoAsync(long userId);
        Task<StudentDto> RegisterStudentAsync(long userId, RegisteredStudentDto student);
        Task<PasswordResetCodeSendingResultDto> SendPasswordResetCodeAsync(string email);
        Task<PasswordResetCodeValidationResultDto> ValidatePasswordResetCodeAsync(PasswordResetCodeDto resetCode);
        Task<bool> ResetPasswordAsync(NewPasswordDto newPassword);
    }
}