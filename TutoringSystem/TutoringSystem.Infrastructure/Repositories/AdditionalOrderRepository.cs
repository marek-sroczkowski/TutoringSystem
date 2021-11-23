using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
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
            order.IsActive = false;

            return await UpdateAdditionalOrderAsync(order);
        }

        public async Task<AdditionalOrder> GetAdditionalOrderAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));

            var order = await DbContext.AdditionalOrders
                .Include(o => o.Tutor)
                .FirstOrDefaultAsync(expression);

            return order;
        }

        public IQueryable<AdditionalOrder> GetAdditionalOrdersCollection(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));

            var orders = FindByCondition(expression);

            return orders;
        }

        public async Task<bool> UpdateAdditionalOrderAsync(AdditionalOrder updatedOrder)
        {
            Update(updatedOrder);

            return await SaveChangedAsync();
        }
    }
}
