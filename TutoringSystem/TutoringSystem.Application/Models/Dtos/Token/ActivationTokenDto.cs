using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Models.Dtos.Token
{
    public class ActivationTokenDto : IMap
    {
        public long Id { get; set; }
        public string TokenContent { get; set; }
        public DateTime ExpirationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ActivationToken, ActivationTokenDto>();
        }
    }
}
