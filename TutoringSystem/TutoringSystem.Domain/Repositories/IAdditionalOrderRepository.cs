using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAdditionalOrderRepository
    {
        Task<bool> AddAdditionalOrderAsync(AdditionalOrder order);
        Task<bool> UpdateAdditionalOrderAsync(AdditionalOrder updatedOrder);
        Task<bool> DeleteAdditionalOrderAsync(AdditionalOrder order);
        Task<AdditionalOrder> GetAdditionalOrderAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActiv = true);
        Task<IEnumerable<AdditionalOrder>> GetAdditionalOrdersCollectionAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActiv = true);
    }
}
