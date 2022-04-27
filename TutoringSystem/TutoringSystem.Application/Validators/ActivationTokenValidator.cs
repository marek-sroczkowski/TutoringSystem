using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Token;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class ActivationTokenValidator : AbstractValidator<ActivationTokenDto>
    {
        private readonly IUserRepository userRepository;
        private readonly IActivationTokenRepository tokenRepository;
        private readonly IHttpContextAccessor httpContext;

        public ActivationTokenValidator(IUserRepository userRepository, IActivationTokenRepository tokenRepository, IHttpContextAccessor httpContext)
        {
            this.userRepository = userRepository;
            this.tokenRepository = tokenRepository;
            this.httpContext = httpContext;

            ValidateTokenExistence();
        }

        private void ValidateTokenExistence()
        {
            RuleFor(s => s.Content).Custom((value, context) =>
            {
                var userId = httpContext.HttpContext.User.GetUserId();
                var user = userRepository.GetUserAsync(u => u.Id == userId && !u.IsEnable).Result;
                if (user is null)
                {
                    context.AddFailure("user", "User has an active account");
                }

                var now = DateTime.Now.ToLocal().AddSeconds(5);
                var userToken = tokenRepository.GetTokenAsync(t => t.ExpirationDate > now && t.TokenContent == value).Result;
                if (userToken is null)
                {
                    context.AddFailure("token", "Invalid activation token");
                }
            });
        }
    }
}