using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Address
{
    public class AddressDto : IMap
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string HouseAndFlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Address, AddressDto>();
        }

        public AddressDto()
        {
        }

        public AddressDto(Domain.Entities.Address address)
        {
            Id = address.Id;
            Street = address.Street;
            HouseAndFlatNumber = address.HouseAndFlatNumber;
            City = address.City;
            PostalCode = address.PostalCode;
        }
    }
}