namespace TutoringSystem.Application.Models.Dtos.Password
{
    public class PasswordResetCodeDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}