using TutoringSystem.Application.Models.Enums;

namespace TutoringSystem.Application.Models.Dtos.Password
{
    public class PasswordResetCodeSendingResultDto
    {
        public PasswordResetCodeSendingResult Result { get; set; }

        public PasswordResetCodeSendingResultDto()
        {
        }

        public PasswordResetCodeSendingResultDto(PasswordResetCodeSendingResult result)
        {
            Result = result;
        }
    }
}