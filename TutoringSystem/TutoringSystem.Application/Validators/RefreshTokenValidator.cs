using FluentValidation;
using Microsoft.AspNetCore.Http;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Validators
{
    public class RefreshTokenValidator : AbstractValidator<JwtRefreshRequestDto>
    {
        public RefreshTokenValidator(IRefreshTokenRepository refreshTokenRepository, IHttpContextAccessor httpContext)
        {
            RuleFor(r => r).Custom((value, context) =>
            {
                var userId = httpContext.HttpContext.User.GetUserId();
                if (!refreshTokenRepository.IsTokenExist(t => t.Token == value.RefreshToken && t.UserId == userId && t.DeviceIdentificator == value.DeviceIdentificator && t.IsActive))
                {
                    context.AddFailure("errors", "Invalid refresh token");
                }
            });
        }
    }
}