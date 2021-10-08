using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AddressDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressDto> AddAddressAsync(long userId, NewAddressDto newAddress);
        Task<AddressDetailsDto> GetAddressByIdAsync(long addressId);
        Task<AddressDetailsDto> GetAddressByUserAsync(long userId);
        Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress);
    }
}