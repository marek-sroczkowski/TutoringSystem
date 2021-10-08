using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
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
            var order = await additionalOrderRepository.GetAdditionalOrderAsync(o => o.Id.Equals(orderId));
            order.Status = orderStatus;

            return await additionalOrderRepository.UpdateAdditionalOrderAsync(order);
        }

        public async Task<bool> DeleteAdditionalOrderAsync(long orderId)
        {
            var order = await additionalOrderRepository.GetAdditionalOrderAsync(o => o.Id.Equals(orderId));

            return await additionalOrderRepository.DeleteAdditionalOrderAsync(order);
        }

        public async Task<OrderDetailsDto> GetAdditionalOrderByIdAsync(long orderId)
        {
            var order = await additionalOrderRepository.GetAdditionalOrderAsync(o => o.Id.Equals(orderId));

            return mapper.Map<OrderDetailsDto>(order);
        }

        public async Task<PagedList<OrderDto>> GetAdditionalOrdersAsync(long tutorId, AdditionalOrderParameters parameters)
        {
            Expression<Func<AdditionalOrder, bool>> expression = o => o.TutorId.Equals(tutorId);
            FilterByStartReceiptDate(ref expression, parameters.ReceiptStartDate);
            FilterByEndReceiptDate(ref expression, parameters.ReceiptEndDate);
            FilterByStatus(ref expression, parameters.Status);
            FilterByPaid(ref expression, parameters.IsPaid);
            FilterByStartDeadline(ref expression, parameters.DeadlineStart);
            FilterByEndDeadline(ref expression, parameters.DeadlineEnd);

            var orders = await additionalOrderRepository.GetAdditionalOrdersCollectionAsync(expression);
            var orderDtos = mapper.Map<ICollection<OrderDto>>(orders);

            return PagedList<OrderDto>.ToPagedList(orderDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder)
        {
            var existingOrder = await additionalOrderRepository.GetAdditionalOrderAsync(o => o.Id.Equals(updatedOrder.Id));
            var order = mapper.Map(updatedOrder, existingOrder);

            return await additionalOrderRepository.UpdateAdditionalOrderAsync(order);
        }

        private void FilterByStatus(ref Expression<Func<AdditionalOrder, bool>> expression, AdditionalOrderStatus? status)
        {
            if (!status.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(status.Value));
        }

        private void FilterByPaid(ref Expression<Func<AdditionalOrder, bool>> expression, bool? isPaid)
        {
            if (!isPaid.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, o => o.IsPaid.Equals(isPaid.Value));
        }

        private void FilterByStartReceiptDate(ref Expression<Func<AdditionalOrder, bool>> expression, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, o => o.ReceiptDate >= startDate.Value);
        }

        private void FilterByEndReceiptDate(ref Expression<Func<AdditionalOrder, bool>> expression, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, o => o.ReceiptDate <= endDate.Value);
        }

        private void FilterByStartDeadline(ref Expression<Func<AdditionalOrder, bool>> expression, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, o => o.Deadline >= startDate.Value);
        }

        private void FilterByEndDeadline(ref Expression<Func<AdditionalOrder, bool>> expression, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, o => o.Deadline <= startDate.Value);
        }
    }
}
