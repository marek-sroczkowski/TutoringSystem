namespace TutoringSystem.Application.Dtos.Enums
{
    public enum WrongPasswordStatus
    {
        DatabaseError = -1,
        PasswordsVary,
        TooShort,
        DuplicateOfOld,
        InvalidOldPassword
    }
}
