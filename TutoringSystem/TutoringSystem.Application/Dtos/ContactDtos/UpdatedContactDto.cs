using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.ContactDtos
{
    public class UpdatedContactDto : IMap
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public ICollection<UpdatedPhoneNumberDto> PhoneNumbers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedContactDto, Contact>();
        }
    }
}
