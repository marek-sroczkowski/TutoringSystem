using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Models.Dtos.Token
{
    public class TokenDto : IMap
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RefreshToken, TokenDto>();
        }
    }
}