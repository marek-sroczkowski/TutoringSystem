namespace TutoringSystem.Application.Dtos.EmailDtos
{
    public class ActivationEmailDto
    {
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public string ActivationToken { get; set; }

        public ActivationEmailDto()
        {
        }

        public ActivationEmailDto(string recipientEmail, string recipientName, string activationToken)
        {
            RecipientEmail = recipientEmail;
            RecipientName = recipientName;
            ActivationToken = activationToken;
        }
    }
}
