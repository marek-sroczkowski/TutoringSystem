using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using TutoringSystem.Application.Dtos.EmailDtos;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendEmail(ActivationEmailDto activationEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration.GetSection("EmailSender").GetValue<string>("Email")));
            email.To.Add(MailboxAddress.Parse(activationEmail.RecipientEmail));
            email.Subject = configuration.GetSection("EmailSender").GetValue<string>("Subject");
            email.Body = new TextPart(TextFormat.Html) { Text = GetActivationEmailContent(activationEmail.RecipientName, activationEmail.ActivationToken), };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration.GetSection("EmailSender").GetValue<string>("Email"), configuration.GetSection("EmailSender").GetValue<string>("Password"));
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
