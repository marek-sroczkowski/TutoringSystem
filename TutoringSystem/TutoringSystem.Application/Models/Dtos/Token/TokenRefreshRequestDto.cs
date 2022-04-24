namespace TutoringSystem.Application.Models.Dtos.Token
{
    public class TokenRefreshRequestDto
    {
        public string Token { get; set; }
        public string DeviceIdentificator { get; set; }

        public TokenRefreshRequestDto()
        {
        }

        public TokenRefreshRequestDto(string token, string deviceIdentificator)
        {
            Token = token;
            DeviceIdentificator = deviceIdentificator;
        }
    }
}