namespace TutoringSystem.Application.Models.Enums
{
    public enum PasswordResetCodeValidationResult
    {
        InternalError = -1,
        Success,
        ExpiredToken,
        IncorrectCode
    }
}