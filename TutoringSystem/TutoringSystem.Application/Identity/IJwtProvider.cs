using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Authentication;

namespace TutoringSystem.Application.Identity
{
    public interface IJwtProvider
    {
        JwtTokenDto GenerateJwtToken(UserDto user);
    }
}