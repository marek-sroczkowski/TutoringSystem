namespace TutoringSystem.Application.Models.Dtos.Email
{
    public class ActivationEmailDto
    {
        public string RecipientEmail { get; set; }
        public string RecipientName { get; set; }
        public string ActivationToken { get; set; }
        public string Subject { get => "Kod aktywacyjny do MS Korepetytor"; }
        public string Content
        {
            get => $"Witaj {RecipientName},<br>" +
                $"Dziękuję za rejestracje w aplikacji MS Korepetytor.<br>" +
                $"Twój kod aktywacyjny to <b>{ActivationToken}</b><br>" +
                $"Uwaga! Nieaktywowane konto zostanie usunięte między 24, a 48h po rejestracji.<br><br>" +
                $"W przypadku jakiś pytań zapraszam do kontaktu,<br>" +
                $"Marek Sroczkowski - twórca aplikacji<br>";
        }

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