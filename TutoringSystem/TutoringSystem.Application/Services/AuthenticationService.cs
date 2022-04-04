using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
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
        private readonly ITutorRepository tutorRepository;
        private readonly IJwtProvider jwtProvider;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IMapper mapper;

        public AuthenticationService(IUserRepository userRepository,
            ITutorRepository tutorRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher<User> passwordHasher,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.tutorRepository = tutorRepository;
            this.jwtProvider = jwtProvider;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
        }

        public async Task<AuthenticationResposneDto> AuthenticateAsync(AuthenticationDto authentication)
        {
            var user = await userRepository.GetUserAsync(u => u.Username.Equals(authentication.Username), isEagerLoadingEnabled: true);

            if (user is null || ValidatePassword(authentication, user) == PasswordVerificationResult.Failed)
            {
                return new AuthenticationResposneDto(AuthenticationStatus.InvalidUsernameOrPassword);
            }

            var userDto = mapper.Map<UserDto>(user);
            var token = jwtProvider.GenerateJwtToken(userDto);
            await SetLastLoginDateAsync(user);

            if (user.IsActive && !user.IsEnable && user.Role == Role.Student && user.PasswordHash is null)
            {
                return new AuthenticationResposneDto(AuthenticationStatus.UnregistredStudent, userDto, token);
            }

            return !user.IsEnable
                ? new AuthenticationResposneDto(AuthenticationStatus.InactiveAccount, userDto, token)
                : new AuthenticationResposneDto(AuthenticationStatus.Success, userDto, token);
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
    }
}