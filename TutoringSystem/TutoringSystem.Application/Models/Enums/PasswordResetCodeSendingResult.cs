namespace TutoringSystem.Application.Models.Enums
{
    public enum PasswordResetCodeSendingResult
    {
        InternalError = -1,
        Success,
        InvalidEmail
    }
}