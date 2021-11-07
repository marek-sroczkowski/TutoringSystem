using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAdditionalOrderService
    {
        Task<PagedList<OrderDto>> GetAdditionalOrdersAsync(long tutorId, AdditionalOrderParameters parameters);
        Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId);
        Task<OrderDto> AddAdditionalOrderAsync(long tutorId, NewOrderDto newOrder);
        Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder);
        Task<bool> DeleteAdditionalOrderAsync(long orderId);
        Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus orderStatus);
        Task<bool> ChangePaymentStatusAsync(long orderId, bool isPaid);
    }
}
