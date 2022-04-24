using FluentValidation;
using TutoringSystem.Application.Models.Dtos.Token;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RefreshTokenValidator : AbstractValidator<TokenRefreshRequestDto>
    {
        private readonly IRefreshTokenRepository refreshTokenRepository;

        public RefreshTokenValidator(IRefreshTokenRepository refreshTokenRepository)
        {
            this.refreshTokenRepository = refreshTokenRepository;
            ValidateTokenExistence();
        }

        private void ValidateTokenExistence()
        {
            RuleFor(r => r).Custom((value, context) =>
            {
                if (!refreshTokenRepository.TokenExists(t => t.Token == value.Token && t.DeviceIdentificator == value.DeviceIdentificator && t.IsActive))
                {
                    context.AddFailure("errors", "Invalid refresh token");
                }
            });
        }
    }
}