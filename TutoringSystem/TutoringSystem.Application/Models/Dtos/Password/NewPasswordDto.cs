namespace TutoringSystem.Application.Models.Dtos.Password
{
    public class NewPasswordDto
    {
        public string ResetCode { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}