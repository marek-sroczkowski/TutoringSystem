using System;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.IdentityDtos;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class AuthenticationResposneDto
    {
        public AuthenticationStatus Status { get; set; }
        public UserDto User { get; set; }
        public JwtToken Token { get; set; }

        public AuthenticationResposneDto()
        {
        }

        public AuthenticationResposneDto(AuthenticationStatus status)
        {
            Status = status;
        }

        public AuthenticationResposneDto(AuthenticationStatus status, UserDto user, JwtToken token)
        {
            Status = status;
            User = user;
            Token = token;
        }
    }
}