using TutoringSystem.Application.Dtos.EmailDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(ActivationEmailDto activationEmail);
    }
}
