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
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AddressService(IAddressRepository addressRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.addressRepository = addressRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<AddressDetailsDto> GetAddressByIdAsync(long addressId)
        {
            var address = await addressRepository.GetAddressAsync(a => a.Id.Equals(addressId));
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(address.UserId));
            var addressDto = mapper.Map<AddressDetailsDto>(address);
            addressDto.Owner = $"{user.FirstName} {user.LastName}";

            return addressDto;
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
