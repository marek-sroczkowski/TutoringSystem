using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
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
                email.Subject = activationEmail.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = activationEmail.Content, };

                return await SendEmailAsync(email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendPasswordResetCodeAsync(PasswordResetEmailDto resetEmail)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(settings.SmtpEmail));
                email.To.Add(MailboxAddress.Parse(resetEmail.RecipientEmail));
                email.Subject = resetEmail.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = resetEmail.Content, };

                return await SendEmailAsync(email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<bool> SendEmailAsync(MimeMessage email)
        {
            try
            {
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(settings.SmtpHost, settings.SmtpPort, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(settings.SmtpEmail, settings.SmtpPassword);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
