using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.PhoneNumber
{
    public class NewPhoneNumberDto : IMap
    {
        public string Owner { get; set; }
        public string Number { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewPhoneNumberDto, Domain.Entities.PhoneNumber>();
        }
    }
}
