using FluentValidation;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RefreshTokenValidator : AbstractValidator<TokenRefreshRequestDto>
    {
        public RefreshTokenValidator(IRefreshTokenRepository refreshTokenRepository)
        {
            RuleFor(r => r).Custom((value, context) =>
            {
                if (!refreshTokenRepository.IsTokenExist(t => t.Token == value.Token && t.DeviceIdentificator == value.DeviceIdentificator && t.IsActive))
                {
                    context.AddFailure("errors", "Invalid refresh token");
                }
            });
        }
    }
}