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
    public class AdditionalOrderRepository : RepositoryBase<AdditionalOrder>, IAdditionalOrderRepository
    {
        public AdditionalOrderRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddOrderAsync(AdditionalOrder order)
        {
            Create(order);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddOrdersCollectionAsync(IEnumerable<AdditionalOrder> orders)
        {
            CreateRange(orders);

            return await SaveChangedAsync();
        }

        public async Task<AdditionalOrder> GetOrderAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var order = isEagerLoadingEnabled
                ? await GetOrderWithEagerLoadingAsync(expression)
                : await GetOrderWithoutEagerLoadingAsync(expression);

            return order;
        }

        public IQueryable<AdditionalOrder> GetOrdersCollection(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var orders = !isEagerLoadingEnabled
                ? Find(expression)
                : GetOrdersCollectionWithEagerLoading(expression);

            return orders;
        }

        public async Task<IEnumerable<AdditionalOrder>> GetOrdersCollectionAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var orders = !isEagerLoadingEnabled
                ? Find(expression)
                : GetOrdersCollectionWithEagerLoading(expression);

            return await orders.ToListAsync();
        }

        public bool IsOrderExist(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveOrderAsync(AdditionalOrder order)
        {
            order.IsActive = false;

            return await UpdateOrderAsync(order);
        }

        public async Task<bool> UpdateOrderAsync(AdditionalOrder order)
        {
            Update(order);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateOrdersCollectionAsync(IEnumerable<AdditionalOrder> orders)
        {
            UpdateRange(orders);

            return await SaveChangedAsync();
        }

        private async Task<AdditionalOrder> GetOrderWithEagerLoadingAsync(Expression<Func<AdditionalOrder, bool>> expression)
        {
            var order = await Find(expression)
                .Include(o => o.Tutor)
                .FirstOrDefaultAsync();

            return order;
        }

        private async Task<AdditionalOrder> GetOrderWithoutEagerLoadingAsync(Expression<Func<AdditionalOrder, bool>> expression)
        {
            var order = await Find(expression)
                .FirstOrDefaultAsync();

            return order;
        }

        private IQueryable<AdditionalOrder> GetOrdersCollectionWithEagerLoading(Expression<Func<AdditionalOrder, bool>> expression)
        {
            var orders = Find(expression)
                .Include(o => o.Tutor);

            return orders;
        }
    }
}