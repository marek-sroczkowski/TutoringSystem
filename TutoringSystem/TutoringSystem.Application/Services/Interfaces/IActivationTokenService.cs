using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Token;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IActivationTokenService
    {
        Task<ActivationTokenDto> AddActivationTokenAsync(long userId);
    }
}
