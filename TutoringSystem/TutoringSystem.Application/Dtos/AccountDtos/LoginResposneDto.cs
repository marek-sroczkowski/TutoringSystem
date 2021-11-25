using TutoringSystem.Application.Dtos.Enums;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class LoginResposneDto
    {
        public LoginStatus LoginStatus { get; set; }
        public UserDto User { get; set; }

        public LoginResposneDto()
        {
        }

        public LoginResposneDto(LoginStatus loginStatus, UserDto user)
        {
            LoginStatus = loginStatus;
            User = user;
        }
    }
}
