using AutoMapper;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IJwtProvider jwtProvider;
        private readonly IMapper mapper;

        public RefreshTokenService(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtProvider jwtProvider,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.jwtProvider = jwtProvider;
            this.mapper = mapper;
        }

        public async Task<JwtTokenDto> GenerateRefreshedJwtTokenAsync(long userId, JwtRefreshRequestDto jwtRefreshRequest)
        {
            var user = await userRepository.GetUserAsync(u => u.Id == userId, isEagerLoadingEnabled: true);
            var userToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);

            return jwtProvider.GenerateJwtToken(mapper.Map<UserDto>(user));
        }

        public async Task<RefreshTokenDto> AddRefreshToken(long userId, string deviceIdentificator, string clientIp)
        {
            var oldToken = await GetTokenByUserAsync(userId, deviceIdentificator);
            var newToken = GenerateRefreshToken(userId, deviceIdentificator, clientIp);
            await refreshTokenRepository.AddTokenAsync(newToken);
            await RemoveOldRefreshToken(oldToken, newToken);

            return mapper.Map<RefreshTokenDto>(newToken);
        }

        public async Task<RefreshToken> GetTokenByUserAsync(long userId, string deviceIdentificator)
        {
            var token = await refreshTokenRepository.GetTokenAsync(t => t.UserId == userId && t.IsActive && t.DeviceIdentificator == deviceIdentificator);

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

        private static RefreshToken GenerateRefreshToken(long userId, string deviceIdentificator, string clientIp)
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
                ExpiresDate = DateTime.Now.ToLocal().AddDays(30),
            };
        }
    }
}