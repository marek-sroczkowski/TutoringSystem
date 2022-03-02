using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> AddAddressesCollectionAsync(IEnumerable<Address> addresses)
        {
            CreateRange(addresses);

            return await SaveChangedAsync();
        }

        public async Task<Address> GetAddressAsync(Expression<Func<Address, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var address = isEagerLoadingEnabled
                ? await GetAddressWithEagerLoadingAsync(expression)
                : await GetAddressWithoutEagerLoadingAsync(expression);

            return address;
        }

        public IQueryable<Address> GetAddressesCollection(Expression<Func<Address, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            ExpressionMerger.MergeExpression(ref expression, a => a.User.IsActive);

            var addresses = isEagerLoadingEnabled
                ? GetAddressesCollectionWithEagerLoading(expression)
                : Find(expression);

            return addresses;
        }

        public async Task<IEnumerable<Address>> GetAddressesCollectionAsync(Expression<Func<Address, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            ExpressionMerger.MergeExpression(ref expression, a => a.User.IsActive);

            var addresses = isEagerLoadingEnabled
                ? GetAddressesCollectionWithEagerLoading(expression)
                : Find(expression);

            return await addresses.ToListAsync();
        }

        public bool IsAddressExist(Expression<Func<Address, bool>> expression)
        {
            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveAddressAsync(Address address)
        {
            Delete(address);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateAddressAsync(Address address)
        {
            Update(address);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateAddressesCollectionAsync(IEnumerable<Address> addresses)
        {
            UpdateRange(addresses);

            return await SaveChangedAsync();
        }

        private async Task<Address> GetAddressWithEagerLoadingAsync(Expression<Func<Address, bool>> expression)
        {
            var address = await Find(expression)
                .Include(a => a.User)
                .FirstOrDefaultAsync();

            return address;
        }

        private async Task<Address> GetAddressWithoutEagerLoadingAsync(Expression<Func<Address, bool>> expression)
        {
            var address = await Find(expression)
                .FirstOrDefaultAsync();

            return address;
        }

        private IQueryable<Address> GetAddressesCollectionWithEagerLoading(Expression<Func<Address, bool>> expression)
        {
            var addresses = Find(expression)
                .Include(a => a.User);

            return addresses;
        }
    }
}
