using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Application.Extensions;

namespace TutoringSystem.Application.Identity
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions jwtOptions;

        public JwtProvider(JwtOptions jwtOptions)
        {
            this.jwtOptions = jwtOptions;
        }

        public TokenDto GenerateJwtToken(UserDto user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.ToLocal().AddDays(jwtOptions.JwtExpireDays);

            var token = new JwtSecurityToken
            (
                jwtOptions.JwtIssuer,
                jwtOptions.JwtIssuer,
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