using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task<bool> SendActivationCodeAsync(ActivationEmailDto activationEmail)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(settings.SmtpEmail));
                email.To.Add(MailboxAddress.Parse(activationEmail.RecipientEmail));
                email.Subject = settings.ActivationEmailSubject;
                email.Body = new TextPart(TextFormat.Html) { Text = GetActivationEmailContent(activationEmail.RecipientName, activationEmail.ActivationToken), };

                return await SendEmailAsync(email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> GetGoogleAccessTokenAsync()
        {
            try
            {
                string[] scopes = new string[] { "https://mail.google.com/" };
                ClientSecrets clientSecrets = new()
                {
                    ClientId = settings.GoogleOAuthClientId,
                    ClientSecret = settings.GoogleOAuthClientSecret
                };

                UserCredential userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "user", CancellationToken.None);
                if (userCredential.Token.IsExpired(userCredential.Flow.Clock))
                {
                    if (!await userCredential.RefreshTokenAsync(CancellationToken.None))
                    {
                        return null;
                    }
                }

                return userCredential.Token.AccessToken;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async Task<bool> SendEmailAsync(MimeMessage email)
        {
            try
            {
                var accessToken = await GetGoogleAccessTokenAsync();
                if (string.IsNullOrEmpty(accessToken))
                {
                    return false;
                }

                using var client = new SmtpClient();
                client.Connect(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.StartTls);
                var oauth2 = new SaslMechanismOAuth2(settings.SmtpEmail, accessToken);
                await client.AuthenticateAsync(oauth2);
                await client.SendAsync(email);
                client.Disconnect(true);

                //using var smtp = new SmtpClient();
                //smtp.Connect(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.StartTls);
                //smtp.Authenticate(settings.SmtpEmail, settings.SmtpPassword);
                //smtp.Send(email);
                //smtp.Disconnect(true);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
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
