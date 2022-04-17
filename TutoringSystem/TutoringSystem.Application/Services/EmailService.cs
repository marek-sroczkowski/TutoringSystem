using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using TutoringSystem.Application.Dtos.EmailDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings settings;

        public EmailService(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        public void SendEmail(ActivationEmailDto activationEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(settings.SmtpEmail));
            email.To.Add(MailboxAddress.Parse(activationEmail.RecipientEmail));
            email.Subject = settings.ActivationEmailSubject;
            email.Body = new TextPart(TextFormat.Html) { Text = GetActivationEmailContent(activationEmail.RecipientName, activationEmail.ActivationToken), };

            using var smtp = new SmtpClient();
            smtp.Connect(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(settings.SmtpEmail, settings.SmtpPassword);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        private static string GetActivationEmailContent(string name, string activationToken)
        {
            return $"Witaj {name},<br>" +
                $"Dziękuję za rejestracje w aplikacji MS Korepetytor.<br>" +
                $"Twój kod aktywacyjny to <b>{activationToken}</b><br>" +
                $"Jeśli nie aktywujesz konta przez 24h po rejestracji zostanie ono usunięte.<br><br>" +
                $"W przypadku jakiś pytań zapraszam do kontaktu,<br>" +
                $"Marek Sroczkowski - twórca aplikacji<br>";
        }
    }
}
