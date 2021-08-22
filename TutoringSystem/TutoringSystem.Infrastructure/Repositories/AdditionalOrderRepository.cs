using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class AdditionalOrderRepository : RepositoryBase<AdditionalOrder>, IAdditionalOrderRepository
    {
        public AdditionalOrderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddAdditionalOrderAsync(AdditionalOrder order)
        {
            Create(order);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteAdditionalOrderAsync(AdditionalOrder order)
        {
            Delete(order);

            return await SaveChangedAsync();
        }

        public async Task<AdditionalOrder> GetAdditionalOrderByIdAsync(long orderId)
        {
            var order = await DbContext.AdditionalOrders
                .Include(o => o.Tutor)
                .FirstOrDefaultAsync(o => o.Id.Equals(orderId));

            return order;
        }

        public async Task<IEnumerable<AdditionalOrder>> GetAdditionalOrdersAsync(long tutorId)
        {
            var orders = FindByCondition(o => o.TutorId.Equals(tutorId));

            return await orders.ToListAsync();
        }

        public async Task<bool> UpdateAdditionalOrderAsync(AdditionalOrder updatedOrder)
        {
            Update(updatedOrder);

            return await SaveChangedAsync();
        }
    }
}
