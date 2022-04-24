using AutoMapper;
using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Address;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository addressRepository;
        private readonly IMapper mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.mapper = mapper;
        }

        public async Task<AddressDetailsDto> GetAddressByIdAsync(long addressId)
        {
            var address = await addressRepository.GetAddressAsync(a => a.Id.Equals(addressId), true);

            return mapper.Map<AddressDetailsDto>(address);
        }

        public async Task<AddressDto> GetAddressByUserAsync(long userId)
        {
            var address = await addressRepository.GetAddressAsync(a => a.UserId.Equals(userId));

            return mapper.Map<AddressDto>(address);
        }

        public async Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress)
        {
            var existingAddress = await addressRepository.GetAddressAsync(a => a.Id.Equals(updatedAddress.Id));
            var address = mapper.Map(updatedAddress, existingAddress);

            return await addressRepository.UpdateAddressAsync(address);
        }
    }
}
