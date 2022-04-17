using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.ActivationTokenDtos
{
    public class NewActivationTokenDto : IMap
    {
        public string TokenContent { get; set; }
        public long UserId { get; set; }
        public DateTime ExpirationDate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewActivationTokenDto, ActivationToken>();
        }
    }
}
