using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Application.Models.Dtos.PhoneNumber;

namespace TutoringSystem.Application.Models.Dtos.Contact
{
    public class ContactDto : IMap
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DiscordName { get; set; }

        public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Contact, ContactDto>();
        }

        public ContactDto()
        {
        }

        public ContactDto(Domain.Entities.Contact contact)
        {
            Id = contact.Id;
            Email = contact.Email;
            DiscordName = contact.DiscordName;

            PhoneNumbers = contact.PhoneNumbers?.Select(p => new PhoneNumberDto(p));
        }
    }
}
