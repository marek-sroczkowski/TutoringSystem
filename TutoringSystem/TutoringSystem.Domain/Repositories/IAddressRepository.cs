using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAddressRepository
    {
        Task<bool> AddAddressAsync(Address address);
        Task<Address> GetAddressByIdAsync(long addressId);
        Task<bool> UpdateAddressAsync(Address updatedAddress);
        Task<bool> DeleteAddressAsync(Address address);
        Task<Address> GetAddressByUserIdAsync(long userId);
        Task<IEnumerable<Address>> GetAddressesAsync(Expression<Func<Address, bool>> expression);
    }
}
