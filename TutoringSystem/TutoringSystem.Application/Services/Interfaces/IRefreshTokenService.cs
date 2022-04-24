using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Token;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<TokenDto> AddRefreshTokenAsync(TokenRefreshRequestDto refreshData, string clientIp);
        Task<TokenDto> AddRefreshTokenAsync(long userId, string deviceIdentificator, string clientIp);
        Task<TokenDto> GenerateRefreshedJwtTokenAsync(TokenRefreshRequestDto refreshData);
    }
}