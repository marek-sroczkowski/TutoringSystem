using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAddressRepository
    {
        Task<bool> AddAddressAsync(Address address);
        Task<bool> AddAddressesCollectionAsync(IEnumerable<Address> addresses);
        Task<Address> GetAddressAsync(Expression<Func<Address, bool>> expression, bool isEagerLoadingEnabled = false);
        IQueryable<Address> GetAddressesCollection(Expression<Func<Address, bool>> expression, bool isEagerLoadingEnabled = false);
        bool IsAddressExist(Expression<Func<Address, bool>> expression);
        Task<bool> RemoveAddressAsync(Address address);
        Task<bool> UpdateAddressAsync(Address updatedAddress);
        Task<bool> UpdateAddressesCollectionAsync(IEnumerable<Address> addresses);
    }
}