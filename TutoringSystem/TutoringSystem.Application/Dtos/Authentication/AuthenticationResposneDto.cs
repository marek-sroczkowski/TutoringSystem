using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;

namespace TutoringSystem.Application.Dtos.Authentication
{
    public class AuthenticationResposneDto
    {
        public AuthenticationStatus Status { get; set; }
        public UserDto User { get; set; }
        public JwtTokenDto Token { get; set; }

        public AuthenticationResposneDto()
        {
        }

        public AuthenticationResposneDto(AuthenticationStatus status)
        {
            Status = status;
        }

        public AuthenticationResposneDto(AuthenticationStatus status, UserDto user, JwtTokenDto token)
        {
            Status = status;
            User = user;
            Token = token;
        }
    }
}