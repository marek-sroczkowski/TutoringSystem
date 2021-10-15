using TutoringSystem.Application.Dtos.Enums;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class LoginResultDto
    {
        public UserDto User { get; set; }
        public LoginStatus Status { get; set; }

        public LoginResultDto(UserDto user, LoginStatus status)
        {
            User = user;
            Status = status;
        }
    }
}
