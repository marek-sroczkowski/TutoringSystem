using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAdditionalOrderService
    {
        PagedList<OrderDto> GetAdditionalOrders(long tutorId, AdditionalOrderParameters parameters);
        Task<OrderDetailsDto> GetOrderByIdAsync(long orderId);
        Task<OrderDto> AddOrderAsync(long tutorId, NewOrderDto newOrder);
        Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder);
        Task<bool> RemoveOrderAsync(long orderId);
        Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus orderStatus);
        Task<bool> ChangePaymentStatusAsync(long orderId, bool isPaid);
    }
}
