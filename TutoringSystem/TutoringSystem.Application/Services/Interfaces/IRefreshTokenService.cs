using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Authentication;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<TokenDto> AddRefreshTokenAsync(TokenRefreshRequestDto refreshData, string clientIp);
        Task<TokenDto> AddRefreshTokenAsync(long userId, string deviceIdentificator, string clientIp);
        Task<TokenDto> GenerateRefreshedJwtTokenAsync(TokenRefreshRequestDto refreshData);
    }
}