using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Helpers;
using TutoringSystem.Domain.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAdditionalOrderService
    {
        Task<PagedList<OrderDto>> GetAdditionalOrdersAsync(long tutorId, AdditionalOrderParameters parameters);
        Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId);
        Task<OrderDto> AddAdditionalOrderAsync(long tutorId, NewOrderDto newOrder);
        Task<bool> UpdateAdditionalOrderAsync(long orderId, UpdatedOrderDto updatedOrder);
        Task<bool> DeleteAdditionalOrderAsync(long orderId);
        Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus orderStatus);
    }
}
