using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Email;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendActivationCodeAsync(ActivationEmailDto activationEmail);
        Task<bool> SendPasswordResetCodeAsync(PasswordResetEmailDto resetEmail);
    }
}