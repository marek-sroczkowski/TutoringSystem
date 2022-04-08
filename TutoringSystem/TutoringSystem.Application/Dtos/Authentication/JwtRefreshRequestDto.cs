namespace TutoringSystem.Application.Dtos.Authentication
{
    public class JwtRefreshRequestDto
    {
        public string RefreshToken { get; set; }
        public string DeviceIdentificator { get; set; }
    }
}