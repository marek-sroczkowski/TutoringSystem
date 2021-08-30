using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            var address = await DbContext.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id.Equals(addressId));

            return address;
        }

        public async Task<bool> UpdateAddressAsync(Address updatedAddress)
        {
            Update(updatedAddress);

            return await SaveChangedAsync();
        }
    }
}
