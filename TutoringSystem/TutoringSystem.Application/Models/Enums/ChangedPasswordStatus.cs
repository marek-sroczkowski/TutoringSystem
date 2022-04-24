namespace TutoringSystem.Application.Models.Enums
{
    public enum WrongPasswordStatus
    {
        InternalError = -1,
        PasswordsVary,
        NotMeetRequirements,
        DuplicateOfOld,
        InvalidOldPassword,
        InvalidEmail,
        InvalidResetCode
    }
}