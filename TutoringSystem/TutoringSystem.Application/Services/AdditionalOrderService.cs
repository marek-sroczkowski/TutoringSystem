using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Helpers;
using TutoringSystem.Domain.Parameters;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class AdditionalOrderService : IAdditionalOrderService
    {
        private readonly IAdditionalOrderRepository additionalOrderRepository;
        private readonly IMapper mapper;

        public AdditionalOrderService(IAdditionalOrderRepository additionalOrderRepository, IMapper mapper)
        {
            this.additionalOrderRepository = additionalOrderRepository;
            this.mapper = mapper;
        }

        public async Task<OrderDto> AddAdditionalOrderAsync(long tutorId, NewOrderDto newOrder)
        {
            var order = mapper.Map<AdditionalOrder>(newOrder);
            order.TutorId = tutorId;
            var created = await additionalOrderRepository.AddAdditionalOrderAsync(order);

            if (!created)
                return null;

            return mapper.Map<OrderDto>(order);
        }

        public async Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus orderStatus)
        {
            var order = await additionalOrderRepository.GetAdditionalOrderByIdAsync(orderId);
            order.Status = orderStatus;

            return await additionalOrderRepository.UpdateAdditionalOrderAsync(order);
        }

        public async Task<bool> DeleteAdditionalOrderAsync(long orderId)
        {
            var order = await additionalOrderRepository.GetAdditionalOrderByIdAsync(orderId);

            return await additionalOrderRepository.DeleteAdditionalOrderAsync(order);
        }

        public async Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId)
        {
            var order = await additionalOrderRepository.GetAdditionalOrderByIdAsync(orderId);

            return mapper.Map<OrderDetailsDto>(order);
        }

        public async Task<PagedList<OrderDto>> GetAdditionalOrdersAsync(long tutorId, AdditionalOrderParameters parameters)
        {
            var orders = await additionalOrderRepository.GetAdditionalOrdersAsync(tutorId);

            FilterByStartReceiptDate(ref orders, parameters.ReceiptStartDate);
            FilterByEndReceiptDate(ref orders, parameters.ReceiptEndDate);
            FilterByStatus(ref orders, parameters.Status);
            FilterByPaid(ref orders, parameters.IsPaid);
            FilterByStartDeadline(ref orders, parameters.DeadlineStart);
            FilterByEndDeadline(ref orders, parameters.DeadlineEnd);

            var orderDtos = mapper.Map<ICollection<OrderDto>>(orders);

            return PagedList<OrderDto>.ToPagedList(orderDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder)
        {
            var existingOrder = await additionalOrderRepository.GetAdditionalOrderByIdAsync(updatedOrder.Id);
            var order = mapper.Map(updatedOrder, existingOrder);

            return await additionalOrderRepository.UpdateAdditionalOrderAsync(order);
        }

        private void FilterByStatus(ref IEnumerable<AdditionalOrder> orders, AdditionalOrderStatus? status)
        {
            if (!status.HasValue)
                return;

            orders = orders.Where(o => o.Status.Equals(status.Value));
        }

        private void FilterByPaid(ref IEnumerable<AdditionalOrder> orders, bool? isPaid)
        {
            if (!isPaid.HasValue)
                return;

            orders = orders.Where(o => o.IsPaid.Equals(isPaid.Value));
        }

        private void FilterByStartReceiptDate(ref IEnumerable<AdditionalOrder> orders, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            orders = orders.Where(o => o.ReceiptDate >= startDate.Value);
        }

        private void FilterByEndReceiptDate(ref IEnumerable<AdditionalOrder> orders, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            orders = orders.Where(o => o.ReceiptDate <= endDate.Value);
        }

        private void FilterByStartDeadline(ref IEnumerable<AdditionalOrder> orders, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            orders = orders.Where(o => o.Deadline >= startDate.Value);
        }

        private void FilterByEndDeadline(ref IEnumerable<AdditionalOrder> orders, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            orders = orders.Where(o => o.Deadline <= startDate.Value);
        }
    }
}
