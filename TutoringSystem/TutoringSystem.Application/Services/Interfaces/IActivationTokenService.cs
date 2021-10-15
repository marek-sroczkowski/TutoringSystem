using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ActivationTokenDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IActivationTokenService
    {
        Task<ActivationTokenDto> AddActivationTokenAsync(long userId);
    }
}
