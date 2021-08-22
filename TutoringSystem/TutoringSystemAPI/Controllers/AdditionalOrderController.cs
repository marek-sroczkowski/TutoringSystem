﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Parameters;
using TutoringSystem.Application.Filters;
using TutoringSystem.Application.Authorization;

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
        public async Task<ActionResult<List<OrderDto>>> Get([FromQuery] AdditionalOrderParameters parameters)
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var orders = await additionalOrderService.GetAdditionalOrdersAsync(long.Parse(tutorId), parameters);

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
        public async Task<ActionResult<OrderDetailsDto>> Get(int orderId)
        {
            var album = await additionalOrderService.GetAdditionalOrderByIdAsync(orderId);

            var authorizationResult = authorizationService.AuthorizeAsync(User, album, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            return Ok(album);
        }

        [SwaggerOperation(Summary = "Creates a new order")]
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> Post([FromBody] CreateOrderDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var order = await additionalOrderService.AddAdditionalOrderAsync(long.Parse(tutorId), model);
            if (order is null)
                return BadRequest("New order could be not added");

            return Created("api/order/" + order.Id, null);
        }

        [SwaggerOperation(Summary = "Updates a existing order")]
        [HttpPut("{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> Put(int orderId, [FromBody] UpdateOrderDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await additionalOrderService.GetAdditionalOrderByIdAsync(orderId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, order, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var updated = await additionalOrderService.UpdateAdditionalOrderAsync(orderId, model);
            if (!updated)
                return BadRequest("Order could be not updated");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Deletes a specific order")]
        [HttpDelete("{orderId}")]
        [ValidateOrderExistence]
        public async Task<ActionResult> Delete(int orderId)
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
    }
}