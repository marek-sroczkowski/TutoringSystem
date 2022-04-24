using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.PhoneNumber
{
    public class PhoneNumberDto : IMap
    {
        public long Id { get; set; }
        public string Owner { get; set; }
        public string Number { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.PhoneNumber, PhoneNumberDto>();
        }

        public PhoneNumberDto()
        {
        }

        public PhoneNumberDto(Domain.Entities.PhoneNumber phone)
        {
            Id = phone.Id;
            Owner = phone.Owner;
            Number = phone.Number;
        }
    }
}
