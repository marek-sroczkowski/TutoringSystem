using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.ContactDtos
{
    public class ContactDto : IMap
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Contact, ContactDto>();
        }

        public ContactDto()
        {
        }

        public ContactDto(Contact contact)
        {
            Id = contact.Id;
            Email = contact.Email;
            DiscordName = contact.DiscordName;

            PhoneNumbers = contact.PhoneNumbers?.Select(p => new PhoneNumberDto(p));
        }
    }
}
