using System;

namespace TutoringSystem.Application.Dtos.Authentication
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}