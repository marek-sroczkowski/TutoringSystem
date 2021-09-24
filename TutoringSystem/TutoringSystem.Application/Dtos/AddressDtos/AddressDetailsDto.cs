using AutoMapper;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AddressDtos
{
    public class AddressDetailsDto : IMap
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string HouseAndFlatNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }

        public UserDto User { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Address, AddressDetailsDto>();
        }
    }
}
