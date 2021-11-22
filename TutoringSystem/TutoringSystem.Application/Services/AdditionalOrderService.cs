﻿using AutoMapper;
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

        public async Task<bool> ChangePaymentStatusAsync(long orderId, bool isPaid)
        {
            var order = await additionalOrderRepository.GetAdditionalOrderAsync(o => o.Id.Equals(orderId));
            order.IsPaid = isPaid;

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
            FilterByDate(ref expression, parameters);
            FilterByPayment(ref expression, parameters);
            FilterByStatus(ref expression, parameters);

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

        private void FilterByStatus(ref Expression<Func<AdditionalOrder, bool>> expression, AdditionalOrderParameters parameters)
        {
            if (parameters.IsInProgress && parameters.IsPending && parameters.IsRealized)
                return;

            if (parameters.IsPending && !parameters.IsInProgress && !parameters.IsRealized)
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Pending));
            else if (!parameters.IsPending && parameters.IsInProgress && !parameters.IsRealized)
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.InProgress));
            else if (!parameters.IsPending && !parameters.IsInProgress && parameters.IsRealized)
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Realized));
            else if (parameters.IsPending && parameters.IsInProgress && !parameters.IsRealized)
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Pending) || o.Status.Equals(AdditionalOrderStatus.InProgress));
            else if (parameters.IsPending && !parameters.IsInProgress && parameters.IsRealized)
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Pending) || o.Status.Equals(AdditionalOrderStatus.Realized));
            else if (!parameters.IsPending && parameters.IsInProgress && parameters.IsRealized)
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.InProgress) || o.Status.Equals(AdditionalOrderStatus.Realized));
        }

        private void FilterByPayment(ref Expression<Func<AdditionalOrder, bool>> expression, AdditionalOrderParameters parameters)
        {
            if (parameters.IsPaid && parameters.IsNotPaid)
                return;

            if (parameters.IsPaid && !parameters.IsNotPaid)
                ExpressionMerger.MergeExpression(ref expression, o => o.IsPaid.Equals(true));
            else if(!parameters.IsPaid && parameters.IsNotPaid)
                ExpressionMerger.MergeExpression(ref expression, o => o.IsPaid.Equals(false));
        }

        private void FilterByDate(ref Expression<Func<AdditionalOrder, bool>> expression, AdditionalOrderParameters parameters)
        {
            ExpressionMerger.MergeExpression(ref expression, o => o.ReceiptDate >= parameters.ReceiptStartDate 
                && o.ReceiptDate <= parameters.ReceiptEndDate
                && o.Deadline >= parameters.DeadlineStart
                && o.Deadline <= parameters.DeadlineEnd);
        }

        private void FilterByEndReceiptDate(ref Expression<Func<AdditionalOrder, bool>> expression, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, o => o.ReceiptDate <= endDate.Value);
        }
    }
}
