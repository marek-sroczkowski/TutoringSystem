using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Authentication;

namespace TutoringSystem.Application.Identity
{
    public interface IJwtProvider
    {
        TokenDto GenerateJwtToken(UserDto user);
    }
}