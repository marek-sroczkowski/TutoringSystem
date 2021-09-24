using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.PhoneNumberDtos
{
    public class UpdatedPhoneNumberDto : IMap
    {
        public long Id { get; set; }
        public string Owner { get; set; }
        public string Number { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedPhoneNumberDto, PhoneNumber>();
        }
    }
}
