using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddAddressAsync(Address address)
        {
            Create(address);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteAddressAsync(Address address)
        {
            Delete(address);

            return await SaveChangedAsync();
        }

        public async Task<Address> GetAddressByIdAsync(long addressId)
        {
            var address = await DbContext.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id.Equals(addressId));

            return address;
        }

        public async Task<Address> GetAddressByUserIdAsync(long userId)
        {
            var address = await DbContext.Addresses
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId.Equals(userId));

            return address;
        }

        public async Task<IEnumerable<Address>> GetAddressesAsync(Expression<Func<Address, bool>> expression)
        {
            ExpressionMerger.MergeExpression(ref expression, a => a.User.IsActiv);
            var addresses = FindByCondition(expression)
                .Include(a => a.User);

            return await addresses.ToListAsync();
        }

        public async Task<bool> UpdateAddressAsync(Address updatedAddress)
        {
            Update(updatedAddress);

            return await SaveChangedAsync();
        }
    }
}
