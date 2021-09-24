using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.ContactDtos
{
    public class NewContactDto : IMap
    {
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public ICollection<NewPhoneNumberDto> PhoneNumbers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewContactDto, Contact>();
        }
    }
}
