using System;

namespace TutoringSystem.Application.Dtos.IdentityDtos
{
    public class JwtToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}