namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class PasswordDto
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
