using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AddressDtos
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
            profile.CreateMap<Address, AddressDto>();
        }

        public AddressDto()
        {
        }

        public AddressDto(Address address)
        {
            Id = address.Id;
            Street = address.Street;
            HouseAndFlatNumber = address.HouseAndFlatNumber;
            City = address.City;
            PostalCode = address.PostalCode;
        }
    }
}