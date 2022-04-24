using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAdditionalOrderRepository
    {
        Task<bool> AddOrderAsync(AdditionalOrder order);
        Task<bool> AddOrdersCollectionAsync(IEnumerable<AdditionalOrder> orders);
        Task<AdditionalOrder> GetOrderAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<AdditionalOrder> GetOrdersCollection(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<AdditionalOrder>> GetOrdersCollectionAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool OrderExists(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true);
        Task<bool> RemoveOrderAsync(AdditionalOrder order);
        Task<bool> UpdateOrderAsync(AdditionalOrder updatedOrder);
        Task<bool> UpdateOrdersCollectionAsync(IEnumerable<AdditionalOrder> orders);
    }
}