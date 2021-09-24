using AutoMapper;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AddressDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
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

        public async Task<AddressDto> AddAddressAsync(long userId, NewAddressDto newAddress)
        {
            var address = mapper.Map<Address>(newAddress);
            address.UserId = userId;

            var created = await addressRepository.AddAddressAsync(address);
            if (!created)
                return null;

            return mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDetailsDto> GetAddressByIdAsync(long addressId)
        {
            var address = await addressRepository.GetAddressByIdAsync(addressId);

            return mapper.Map<AddressDetailsDto>(address);
        }

        public async Task<AddressDetailsDto> GetAddressByUserAsync(long userId)
        {
            var address = await addressRepository.GetAddressByUserIdAsync(userId);

            return mapper.Map<AddressDetailsDto>(address);
        }

        public async Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress)
        {
            var existingAddress = await addressRepository.GetAddressByIdAsync(updatedAddress.Id);
            var address = mapper.Map(updatedAddress, existingAddress);

            return await addressRepository.UpdateAddressAsync(address);
        }

        public async Task<bool> DeleteAddressAsync(long addressId)
        {
            var address = await addressRepository.GetAddressByUserIdAsync(addressId);

            return await addressRepository.DeleteAddressAsync(address);
        }
    }
}
