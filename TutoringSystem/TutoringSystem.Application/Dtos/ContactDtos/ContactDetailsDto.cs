using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.ContactDtos
{
    public class ContactDetailsDto : IMap
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }
        public string Owner { get; set; }

        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Contact, ContactDetailsDto>()
                .ForMember(dest => dest.Owner, map => map.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));
        }
    }
}