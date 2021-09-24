using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.AccountDtos;
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

        public ICollection<PhoneNumberDto> PhoneNumbers { get; set; }

        public virtual UserDto User { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Contact, ContactDetailsDto>();
        }
    }
}
