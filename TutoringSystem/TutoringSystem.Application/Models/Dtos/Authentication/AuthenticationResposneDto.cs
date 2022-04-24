using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Application.Models.Dtos.Token;
using TutoringSystem.Application.Models.Enums;

namespace TutoringSystem.Application.Models.Dtos.Authentication
{
    public class AuthenticationResposneDto
    {
        public AuthenticationStatus Status { get; set; }
        public UserDto User { get; set; }
        public TokenDto JwtToken { get; set; }
        public TokenDto RefreshToken { get; set; }

        public AuthenticationResposneDto()
        {
        }

        public AuthenticationResposneDto(AuthenticationStatus status)
        {
            Status = status;
        }

        public AuthenticationResposneDto(AuthenticationStatus status, UserDto user, TokenDto jwtToken, TokenDto refreshToken)
        {
            Status = status;
            User = user;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}