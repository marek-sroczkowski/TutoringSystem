using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Address;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAddressService
    {
        Task<AddressDetailsDto> GetAddressByIdAsync(long addressId);
        Task<AddressDto> GetAddressByUserAsync(long userId);
        Task<bool> UpdateAddressAsync(UpdatedAddressDto updatedAddress);
    }
}