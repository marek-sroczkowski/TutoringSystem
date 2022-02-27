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
        private readonly ISortHelper<AdditionalOrder> sortHelper;
        private readonly IMapper mapper;

        public AdditionalOrderService(IAdditionalOrderRepository additionalOrderRepository,
            ISortHelper<AdditionalOrder> sortHelper,
            IMapper mapper)
        {
            this.additionalOrderRepository = additionalOrderRepository;
            this.sortHelper = sortHelper;
            this.mapper = mapper;
        }

        public async Task<OrderDto> AddOrderAsync(long tutorId, NewOrderDto newOrder)
        {
            var order = mapper.Map<AdditionalOrder>(newOrder);
            order.TutorId = tutorId;
            var created = await additionalOrderRepository.AddOrderAsync(order);

            return created ? mapper.Map<OrderDto>(order) : null;
        }

        public async Task<bool> ChangeOrderStatusAsync(long orderId, AdditionalOrderStatus orderStatus)
        {
            var order = await additionalOrderRepository.GetOrderAsync(o => o.Id.Equals(orderId));
            order.Status = orderStatus;

            return await additionalOrderRepository.UpdateOrderAsync(order);
        }

        public async Task<bool> ChangePaymentStatusAsync(long orderId, bool isPaid)
        {
            var order = await additionalOrderRepository.GetOrderAsync(o => o.Id.Equals(orderId));
            order.IsPaid = isPaid;

            return await additionalOrderRepository.UpdateOrderAsync(order);
        }

        public async Task<bool> RemoveOrderAsync(long orderId)
        {
            var order = await additionalOrderRepository.GetOrderAsync(o => o.Id.Equals(orderId));

            return await additionalOrderRepository.RemoveOrderAsync(order);
        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(long orderId)
        {
            var order = await additionalOrderRepository.GetOrderAsync(o => o.Id.Equals(orderId));

            return mapper.Map<OrderDetailsDto>(order);
        }

        public PagedList<OrderDto> GetAdditionalOrders(long tutorId, AdditionalOrderParameters parameters)
        {
            var expression = GetExpression(tutorId, parameters);
            var orders = sortHelper.ApplySort(additionalOrderRepository.GetOrdersCollection(expression), parameters.OrderBy);
            var orderDtos = mapper.Map<ICollection<OrderDto>>(orders);

            return PagedList<OrderDto>.ToPagedList(orderDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> UpdateAdditionalOrderAsync(UpdatedOrderDto updatedOrder)
        {
            var existingOrder = await additionalOrderRepository.GetOrderAsync(o => o.Id.Equals(updatedOrder.Id));
            var order = mapper.Map(updatedOrder, existingOrder);

            return await additionalOrderRepository.UpdateOrderAsync(order);
        }

        private static Expression<Func<AdditionalOrder, bool>> GetExpression(long tutorId, AdditionalOrderParameters parameters)
        {
            Expression<Func<AdditionalOrder, bool>> expression = o => o.TutorId.Equals(tutorId);
            FilterByDate(ref expression, parameters);
            FilterByPayment(ref expression, parameters);
            FilterByStatus(ref expression, parameters);

            return expression;
        }

        private static void FilterByStatus(ref Expression<Func<AdditionalOrder, bool>> expression, AdditionalOrderParameters parameters)
        {
            if (parameters.IsInProgress && parameters.IsPending && parameters.IsRealized)
            {
                return;
            }

            if (parameters.IsPending && !parameters.IsInProgress && !parameters.IsRealized)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Pending));
            }
            else if (!parameters.IsPending && parameters.IsInProgress && !parameters.IsRealized)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.InProgress));
            }
            else if (!parameters.IsPending && !parameters.IsInProgress && parameters.IsRealized)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Realized));
            }
            else if (parameters.IsPending && parameters.IsInProgress && !parameters.IsRealized)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Pending) || o.Status.Equals(AdditionalOrderStatus.InProgress));
            }
            else if (parameters.IsPending && !parameters.IsInProgress && parameters.IsRealized)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.Pending) || o.Status.Equals(AdditionalOrderStatus.Realized));
            }
            else if (!parameters.IsPending && parameters.IsInProgress && parameters.IsRealized)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.Status.Equals(AdditionalOrderStatus.InProgress) || o.Status.Equals(AdditionalOrderStatus.Realized));
            }
        }

        private static void FilterByPayment(ref Expression<Func<AdditionalOrder, bool>> expression, AdditionalOrderParameters parameters)
        {
            if (parameters.IsPaid && parameters.IsNotPaid)
            {
                return;
            }

            if (parameters.IsPaid && !parameters.IsNotPaid)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsPaid.Equals(true));
            }
            else if (!parameters.IsPaid && parameters.IsNotPaid)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsPaid.Equals(false));
            }
        }

        private static void FilterByDate(ref Expression<Func<AdditionalOrder, bool>> expression, AdditionalOrderParameters parameters)
        {
            ExpressionMerger.MergeExpression(ref expression, o => o.ReceiptDate.Date >= parameters.ReceiptStartDate.Date
                && o.ReceiptDate.Date <= parameters.ReceiptEndDate.Date
                && o.Deadline.Date >= parameters.DeadlineStart.Date
                && o.Deadline.Date <= parameters.DeadlineEnd.Date);
        }
    }
}
