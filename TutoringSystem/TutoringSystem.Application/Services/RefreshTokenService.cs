using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Application.Models.Dtos.Token;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly AppSettings settings;
        private readonly IJwtProvider jwtProvider;
        private readonly IMapper mapper;

        public RefreshTokenService(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IOptions<AppSettings> settings,
            IJwtProvider jwtProvider,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.settings = settings.Value;
            this.jwtProvider = jwtProvider;
            this.mapper = mapper;
        }

        public async Task<TokenDto> GenerateRefreshedJwtTokenAsync(TokenRefreshRequestDto refreshData)
        {
            var oldToken = await GetRefreshToken(refreshData);
            var user = await userRepository.GetUserAsync(u => u.Id == oldToken.UserId, isEagerLoadingEnabled: true);

            return jwtProvider.GenerateJwtToken(mapper.Map<UserDto>(user));
        }

        public async Task<TokenDto> AddRefreshTokenAsync(TokenRefreshRequestDto refreshData, string clientIp)
        {
            var oldToken = await GetRefreshToken(refreshData);
            var newToken = GenerateRefreshToken(oldToken.UserId, refreshData.DeviceIdentificator, clientIp);
            await refreshTokenRepository.AddTokenAsync(newToken);
            await RemoveOldRefreshToken(oldToken, newToken);

            return mapper.Map<TokenDto>(newToken);
        }

        public async Task<TokenDto> AddRefreshTokenAsync(long userId, string deviceIdentificator, string clientIp)
        {
            var oldToken = await GetTokenByUserAsync(userId, deviceIdentificator);
            var newToken = GenerateRefreshToken(userId, deviceIdentificator, clientIp);
            await refreshTokenRepository.AddTokenAsync(newToken);
            await RemoveOldRefreshToken(oldToken, newToken);

            return mapper.Map<TokenDto>(newToken);
        }

        private async Task<RefreshToken> GetTokenByUserAsync(long userId, string deviceIdentificator)
        {
            var token = await refreshTokenRepository.GetTokenAsync(t => t.UserId == userId && t.DeviceIdentificator == deviceIdentificator && t.IsActive);

            return token;
        }

        private async Task<RefreshToken> GetRefreshToken(TokenRefreshRequestDto refreshData)
        {
            var token = await refreshTokenRepository.GetTokenAsync(t => t.Token == refreshData.Token && t.DeviceIdentificator == refreshData.DeviceIdentificator && t.IsActive);

            return token;
        }

        private async Task RemoveOldRefreshToken(RefreshToken oldToken, RefreshToken newToken)
        {
            if (oldToken is null)
            {
                return;
            }

            oldToken.ReplacedByToken = newToken.Token;
            oldToken.RevokedByIp = newToken.CreatedByIp;
            oldToken.RevokedByDeviceId = newToken.DeviceIdentificator;
            oldToken.RevokedDate = DateTime.Now.ToLocal();

            await refreshTokenRepository.RemoveTokenAsync(oldToken);
        }

        private RefreshToken GenerateRefreshToken(long userId, string deviceIdentificator, string clientIp)
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedByIp = clientIp,
                DeviceIdentificator = deviceIdentificator,
                UserId = userId,
                ExpirationDate = DateTime.Now.ToLocal().AddDays(settings.RefreshTokenExpireDays),
            };
        }
    }
}