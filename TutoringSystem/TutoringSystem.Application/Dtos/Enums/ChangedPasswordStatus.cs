namespace TutoringSystem.Application.Dtos.Enums
{
    public enum WrongPasswordStatus
    {
        InternalError = -1,
        PasswordsVary,
        TooShort,
        DuplicateOfOld,
        InvalidOldPassword
    }
}
