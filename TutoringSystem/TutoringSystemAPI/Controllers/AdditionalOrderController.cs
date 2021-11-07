using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Application.Parameters;
using TutoringSystem.API.Filters.TypeFilters;
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
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders([FromQuery] AdditionalOrderParameters parameters)
        {
            var tutorId = User.GetUserId();
            var orders = await additionalOrderService.GetAdditionalOrdersAsync(tutorId, parameters);

            var metadata = new
            {
                orders.TotalCount,
                orders.PageSize,
                orders.CurrentPage,
                orders.TotalPages,
                orders.HasNext,
                orders.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(orders);
        }

        [SwaggerOperation(Summary = "Retrieves a specific order by unique id")]
        [HttpGet("{orderId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateOrderExistence]
        public async Task<ActionResult<OrderDetailsDto>> GetOrderById(long orderId)
        {
            var order = await additionalOrderService.GetAdditionalOrderByIdAsync(orderId);

            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            return Ok(order);
        }

        [SwaggerOperation(Summary = "Creates a new order")]
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> AddOrder([FromBody] NewOrderDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tutorId = User.GetUserId();
            var order = await additionalOrderService.AddAdditionalOrderAsync(tutorId, model);
            if (order is null)
                return BadRequest("New order could be not added");

            return Created("api/order/" + order.Id, null);
        }

        [SwaggerOperation(Summary = "Updates a existing order")]
        [HttpPut]
        [ValidateOrderExistence]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdatedOrderDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await additionalOrderService.GetAdditionalOrderByIdAsync(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var updated = await additionalOrderService.UpdateAdditionalOrderAsync(model);
            if (!updated)
                return BadRequest("Order could be not updated");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Deletes a specific order")]
        [HttpDelete("{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            var order = await additionalOrderService.GetAdditionalOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var deleted = await additionalOrderService.DeleteAdditionalOrderAsync(orderId);
            if (!deleted)
                return BadRequest("Order could be not deleted");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Changes order status")]
        [HttpPatch("status/{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> ChangeStatus(int orderId, AdditionalOrderStatus status)
        {
            var order = await additionalOrderService.GetAdditionalOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var changed = await additionalOrderService.ChangeOrderStatusAsync(orderId, status);
            if (!changed)
                return BadRequest("Order status could be not changed");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Changes payment status")]
        [HttpPatch("payment/{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> ChangePaymentStatus(int orderId, bool isPaid)
        {
            var order = await additionalOrderService.GetAdditionalOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var changed = await additionalOrderService.ChangePaymentStatusAsync(orderId, isPaid);
            if (!changed)
                return BadRequest("Payment status could be not changed");

            return NoContent();
        }
    }
}
