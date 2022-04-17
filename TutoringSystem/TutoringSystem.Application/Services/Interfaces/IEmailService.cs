using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.EmailDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendActivationCodeAsync(ActivationEmailDto activationEmail);
    }
}