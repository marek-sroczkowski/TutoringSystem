using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAdditionalOrderRepository
    {
        Task<AdditionalOrder> GetAdditionalOrderByIdAsync(long orderId);
        Task<bool> AddAdditionalOrderAsync(AdditionalOrder order);
        Task<bool> UpdateAdditionalOrderAsync(AdditionalOrder updatedOrder);
        Task<bool> DeleteAdditionalOrderAsync(AdditionalOrder order);
        Task<IEnumerable<AdditionalOrder>> GetAdditionalOrdersAsync(Expression<Func<AdditionalOrder, bool>> expression);
    }
}
