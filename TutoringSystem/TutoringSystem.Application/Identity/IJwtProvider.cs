using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.IdentityDtos;

namespace TutoringSystem.Application.Identity
{
    public interface IJwtProvider
    {
        JwtToken GenerateJwtToken(UserDto user);
    }
}