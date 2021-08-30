using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAddressRepository
    {
        Task<bool> AddAddressAsync(Address address);
        Task<Address> GetAddressByIdAsync(int addressId);
        Task<bool> UpdateAddressAsync(Address updatedAddress);
        Task<bool> DeleteAddressAsync(Address address);
    }
}
