using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAdditionalOrderRepository
    {
        Task<IEnumerable<AdditionalOrder>> GetAdditionalOrdersAsync(long tutorId);
        Task<AdditionalOrder> GetAdditionalOrderByIdAsync(long orderId);
        Task<bool> AddAdditionalOrderAsync(AdditionalOrder order);
        Task<bool> UpdateAdditionalOrderAsync(AdditionalOrder updatedOrder);
        Task<bool> DeleteAdditionalOrderAsync(AdditionalOrder order);
    }
}
