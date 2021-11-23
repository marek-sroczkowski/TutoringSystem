using System;
using System.Linq;
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
        Task<AdditionalOrder> GetAdditionalOrderAsync(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true);
        IQueryable<AdditionalOrder> GetAdditionalOrdersCollection(Expression<Func<AdditionalOrder, bool>> expression, bool? isActive = true);
    }
}
