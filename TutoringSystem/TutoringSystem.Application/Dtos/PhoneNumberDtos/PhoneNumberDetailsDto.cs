using AutoMapper;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.PhoneNumberDtos
{
    public class PhoneNumberDetailsDto : IMap
    {
        public string Owner { get; set; }
        public string Number { get; set; }

        public ContactDetailsDto Contact { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PhoneNumber, PhoneNumberDetailsDto>().ReverseMap();
        }
    }
}
