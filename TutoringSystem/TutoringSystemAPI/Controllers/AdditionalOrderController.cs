using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Application.Parameters;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Extensions;

namespace TutoringSystem.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    [Authorize]
    public class AdditionalOrderController : ControllerBase
    {
        private readonly IAdditionalOrderService additionalOrderService;
        private readonly IAuthorizationService authorizationService;

        public AdditionalOrderController(IAdditionalOrderService additionalOrderService, IAuthorizationService authorizationService)
        {
            this.additionalOrderService = additionalOrderService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves all additional order assigned to the currently logged in tutor")]
        [HttpGet]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<OrderDto>> GetOrders([FromQuery] AdditionalOrderParameters parameters)
        {
            var orders = additionalOrderService.GetAdditionalOrders(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", orders.GetPaginationJsonMetadata());

            return Ok(orders);
        }

        [SwaggerOperation(Summary = "Retrieves a specific order by unique id")]
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateOrderExistence]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderById(long orderId)
        {
            var order = await additionalOrderService.GetOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Read)).Result;

            return authorizationResult.Succeeded ? Ok(order) : Forbid();
        }

        [SwaggerOperation(Summary = "Creates a new order")]
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> AddOrder([FromBody] NewOrderDto model)
        {
            var order = await additionalOrderService.AddOrderAsync(User.GetUserId(), model);

            return order != null
                ? Created("api/order/" + order.Id, null)
                : BadRequest("New order could be not added");
        }

        [SwaggerOperation(Summary = "Updates a existing order")]
        [HttpPut]
        [ValidateOrderExistence]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdatedOrderDto model)
        {
            var order = await additionalOrderService.GetOrderByIdAsync(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var updated = await additionalOrderService.UpdateAdditionalOrderAsync(model);

            return updated ? NoContent() : BadRequest("Order could be not updated");
        }

        [SwaggerOperation(Summary = "Deletes a specific order")]
        [HttpDelete("{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            var order = await additionalOrderService.GetOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var deleted = await additionalOrderService.RemoveOrderAsync(orderId);

            return deleted ? NoContent() : BadRequest("Order could be not deleted");
        }

        [SwaggerOperation(Summary = "Changes order status")]
        [HttpPatch("status/{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> ChangeStatus(int orderId, AdditionalOrderStatus status)
        {
            var order = await additionalOrderService.GetOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var changed = await additionalOrderService.ChangeOrderStatusAsync(orderId, status);

            return changed ? NoContent() : BadRequest("Order status could be not changed");
        }

        [SwaggerOperation(Summary = "Changes payment status")]
        [HttpPatch("payment/{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> ChangePaymentStatus(int orderId, bool isPaid)
        {
            var order = await additionalOrderService.GetOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var changed = await additionalOrderService.ChangePaymentStatusAsync(orderId, isPaid);

            return changed ? NoContent() : BadRequest("Payment status could be not changed");
        }
    }
}