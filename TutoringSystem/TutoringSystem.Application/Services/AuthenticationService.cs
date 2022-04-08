﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IJwtProvider jwtProvider;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IMapper mapper;

        public AuthenticationService(IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.refreshTokenRepository = refreshTokenRepository;
            this.jwtProvider = jwtProvider;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
        }

        public async Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto authentication, string clientIp)
        {
            var user = await userRepository.GetUserAsync(u => u.Username.Equals(authentication.Username), isEagerLoadingEnabled: true);

            if (user is null || ValidatePassword(authentication, user) == PasswordVerificationResult.Failed)
            {
                return new AuthenticationResposneDto(AuthenticationStatus.InvalidUsernameOrPassword);
            }

            var userDto = mapper.Map<UserDto>(user);
            var token = jwtProvider.GenerateJwtToken(userDto);
            await SetLastLoginDateAsync(user);
            var refreshToken = await AddRefreshToken(user, authentication.DeviceIdentificator, clientIp);

            if (user.IsActive && !user.IsEnable && user.Role == Role.Student && user.PasswordHash is null)
            {
                return new AuthenticationResposneDto(AuthenticationStatus.UnregistredStudent, userDto, token, refreshToken);
            }

            return !user.IsEnable
                ? new AuthenticationResposneDto(AuthenticationStatus.InactiveAccount, userDto, token, refreshToken)
                : new AuthenticationResposneDto(AuthenticationStatus.Success, userDto, token, refreshToken);
        }

        private async Task<string> AddRefreshToken(User user, string deviceIdentificator, string clientIp)
        {
            var oldToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
            var newToken = GenerateRefreshToken(user.Id, deviceIdentificator, clientIp);
            await refreshTokenRepository.AddTokenAsync(newToken);
            await RemoveOldRefreshTokens(oldToken, newToken);

            return newToken.Token;
        }

        private PasswordVerificationResult? ValidatePassword(AuthenticationDto authenticationModel, User user)
        {
            if (string.IsNullOrEmpty(authenticationModel.Password))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(authenticationModel.Password) && string.IsNullOrEmpty(user.PasswordHash))
            {
                return PasswordVerificationResult.Failed;
            }

            return passwordHasher.VerifyHashedPassword(user, user.PasswordHash, authenticationModel.Password);
        }

        private async Task SetLastLoginDateAsync(User user)
        {
            user.LastLoginDate = DateTime.Now.ToLocal();
            await userRepository.UpdateUserAsync(user);
        }

        private async Task RemoveOldRefreshTokens(RefreshToken oldToken, RefreshToken newToken)
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
                UserId = userId
            };
        }
    }
}