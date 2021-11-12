using AutoMapper;
using System.Collections.Generic;
using System.Linq;
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
        public string Owner { get; set; }

        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }

        public virtual UserDto User { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Contact, ContactDetailsDto>();
        }

        public ContactDetailsDto()
        {
        }

        public ContactDetailsDto(Contact contact)
        {
            Id = contact.Id;
            Email = contact.Email;
            DiscordName = contact.DiscordName;

            PhoneNumbers = contact.PhoneNumbers?.Select(p => new PhoneNumberDto(p));
        }
    }
}
