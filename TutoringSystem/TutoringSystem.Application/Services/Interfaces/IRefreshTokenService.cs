using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenDto> AddRefreshToken(long userId, string deviceIdentificator, string clientIp);
        Task<JwtTokenDto> GenerateRefreshedJwtTokenAsync(long userId, JwtRefreshRequestDto jwtRefreshRequest);
        Task<RefreshToken> GetTokenByUserAsync(long userId, string deviceIdentificator);
    }
}