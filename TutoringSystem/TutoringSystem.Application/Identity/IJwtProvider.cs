using TutoringSystem.Application.Dtos.AccountDtos;

namespace TutoringSystem.Application.Identity
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(UserDto user);
    }
}
