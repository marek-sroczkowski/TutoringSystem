using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Account;
using TutoringSystem.Application.Models.Dtos.Token;

namespace TutoringSystem.Application.Identity
{
    public class JwtProvider : IJwtProvider
    {
        private readonly AppSettings settings;

        public JwtProvider(IOptions<AppSettings> settings)
        {
            this.settings = settings.Value;
        }

        public TokenDto GenerateJwtToken(UserDto user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.ToLocal().AddDays(settings.JwtExpireDays);

            var token = new JwtSecurityToken
            (
                settings.JwtIssuer,
                settings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();

            return new TokenDto
            {
                Token = tokenHandler.WriteToken(token),
                ExpirationDate = expires
            };
        }
    }
}