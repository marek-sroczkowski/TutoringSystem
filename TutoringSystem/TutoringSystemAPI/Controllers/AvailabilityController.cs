using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.AvailabilityDtos;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/availability")]
    [ApiController]
    [Authorize]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService availabilityService;
        private readonly IAuthorizationService authorizationService;

        public AvailabilityController(IAvailabilityService availabilityService, IAuthorizationService authorizationService)
        {
            this.availabilityService = availabilityService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves all availabilities of the current logged in tutor filtered with selected parameters")]
        [HttpGet("all")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<AvailabilityDto>>> GetAllAvailabilities([FromQuery] AvailabilityParameters parameters)
        {
            var availabilities = await availabilityService.GetAvailabilitiesByTutorAsync(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", availabilities.GetPaginationJsonMetadata());

            return Ok(availabilities);
        }

        [SwaggerOperation(Summary = "Retrieves future availabilities of the current logged in tutor filtered with selected parameters")]
        [HttpGet("future")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<AvailabilityDto>>> GetFutureAvailabilities([FromQuery] FutureAvailabilityParameters parameters)
        {
            var availabilities = await availabilityService.GetFutureAvailabilitiesByTutorAsync(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", availabilities.GetPaginationJsonMetadata());

            return Ok(availabilities);
        }

        [SwaggerOperation(Summary = "Retrieves today availabilitiy of the current logged in tutor")]
        [HttpGet("today")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<AvailabilityDetailsDto>> GetTodayAvailability()
        {
            var availability = await availabilityService.GetTodaysAvailabilityByTutorAsync(User.GetUserId());

            return availability != null ? Ok(availability) : NotFound();
        }

        [SwaggerOperation(Summary = "Retrieves a specific availability by unique id")]
        [HttpGet("{availabilityId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateAvailabilityExistence]
        public async Task<ActionResult<AvailabilityDetailsDto>> GetAvailability(long availabilityId)
        {
            var availability = await availabilityService.GetAvailabilityByIdAsync(availabilityId);

            return Ok(availability);
        }

        [SwaggerOperation(Summary = "Added a new availability")]
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> AddAvailability([FromBody] NewAvailabilityDto model)
        {
            var reservation = await availabilityService.AddAvailabilityAsync(User.GetUserId(), model);

            return reservation != null
                ? Created("api/availability/" + reservation.Id, null)
                : BadRequest("New availability could be not added");
        }

        [SwaggerOperation(Summary = "Updates a existing availability")]
        [HttpPut]
        [Authorize(Roles = "Tutor")]
        [ValidateAvailabilityExistence]
        public async Task<ActionResult> UpdateAvailability([FromBody] UpdatedAvailabilityDto model)
        {
            var availability = await availabilityService.GetAvailabilityByIdAsync(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, availability, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var updated = await availabilityService.UpdateAvailabilityAsync(model);

            return updated ? NoContent() : BadRequest("Availability could be not updated");
        }

        [SwaggerOperation(Summary = "Deletes a specific availability")]
        [HttpDelete("{availabilityId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateAvailabilityExistence]
        public async Task<ActionResult> DeleteAvailability(long availabilityId)
        {
            var reservation = await availabilityService.GetAvailabilityByIdAsync(availabilityId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var deleted = await availabilityService.RemoveAvailabilityAsync(availabilityId);

            return deleted ? NoContent() : BadRequest("Availability could be not deleted");
        }
    }
}