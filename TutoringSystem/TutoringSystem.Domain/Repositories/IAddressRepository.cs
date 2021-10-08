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
        Task<bool> UpdateAddressAsync(Address updatedAddress);
        Task<bool> DeleteAddressAsync(Address address);
        Task<Address> GetAddressAsync(Expression<Func<Address, bool>> expression);
        Task<IEnumerable<Address>> GetAddressesCollectionAsync(Expression<Func<Address, bool>> expression);
    }
}
