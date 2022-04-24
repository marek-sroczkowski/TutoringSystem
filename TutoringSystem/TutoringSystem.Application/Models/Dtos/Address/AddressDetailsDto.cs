using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Address
{
    public class AddressDetailsDto : IMap
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string HouseAndFlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Address, AddressDetailsDto>()
                .ForMember(dest => dest.Owner, map => map.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"));
        }
    }
}