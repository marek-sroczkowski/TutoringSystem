namespace TutoringSystem.Application.Models.Dtos.Email
{
    public class PasswordResetEmailDto
    {
        public string RecipientEmail { get; set; }
        public string ResetCode { get; set; }
        public string Subject { get => "Resetowanie hasła"; }
        public string Content
        {
            get => $"Twój kod do resetu hasła to <b>{ResetCode}</b><br>";
        }

        public PasswordResetEmailDto()
        {
        }

        public PasswordResetEmailDto(string recipientEmail, string resetCode)
        {
            RecipientEmail = recipientEmail;
            ResetCode = resetCode;
        }
    }
}