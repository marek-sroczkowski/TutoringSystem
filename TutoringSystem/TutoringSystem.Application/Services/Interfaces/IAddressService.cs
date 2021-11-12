using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AddressDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressDetailsDto> GetAddressByIdAsync(long addressId);
        Task<AddressDto> GetAddressByUserAsync(long userId);
        Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress);
    }
}