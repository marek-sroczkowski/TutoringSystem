using TutoringSystem.Application.Models.Enums;

namespace TutoringSystem.Application.Models.Dtos.Password
{
    public class PasswordResetCodeValidationResultDto
    {
        public PasswordResetCodeValidationResult ValidationResult { get; set; }

        public PasswordResetCodeValidationResultDto()
        {
        }

        public PasswordResetCodeValidationResultDto(PasswordResetCodeValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}