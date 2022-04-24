using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Application.Models.Dtos.Token;

namespace TutoringSystem.Application.Identity
{
    public interface IJwtProvider
    {
        TokenDto GenerateJwtToken(UserDto user);
    }
}