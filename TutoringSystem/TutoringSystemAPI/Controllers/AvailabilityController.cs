using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.API.Filters;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.AvailabilityDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Parameters;

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
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var availabilities = await availabilityService.GetAvailabilitiesByTutorAsync(long.Parse(tutorId), parameters);

            var metadata = new
            {
                availabilities.TotalCount,
                availabilities.PageSize,
                availabilities.CurrentPage,
                availabilities.TotalPages,
                availabilities.HasNext,
                availabilities.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(availabilities);
        }

        [SwaggerOperation(Summary = "Retrieves future availabilities of the current logged in tutor filtered with selected parameters")]
        [HttpGet("future")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<AvailabilityDto>>> GetFutureAvailabilities([FromQuery] FutureAvailabilityParameters parameters)
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var availabilities = await availabilityService.GetFutureAvailabilitiesByTutorAsync(long.Parse(tutorId), parameters);

            var metadata = new
            {
                availabilities.TotalCount,
                availabilities.PageSize,
                availabilities.CurrentPage,
                availabilities.TotalPages,
                availabilities.HasNext,
                availabilities.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(availabilities);
        }

        [SwaggerOperation(Summary = "Retrieves today availabilitiy of the current logged in tutor")]
        [HttpGet("today")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<AvailabilityDetailsDto>> GetTodayAvailability()
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var availability = await availabilityService.GetTodaysAvailabilityByTutorAsync(long.Parse(tutorId));
            if (availability is null)
                return NotFound();

            return Ok(availability);
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var reservation = await availabilityService.AddAvailabilityAsync(long.Parse(tutorId), model);
            if (reservation is null)
                return BadRequest("New availability could be not added");

            return Created("api/availability/" + reservation.Id, null);
        }

        [SwaggerOperation(Summary = "Updates a existing availability")]
        [HttpPut]
        [Authorize(Roles = "Tutor")]
        [ValidateAvailabilityExistence]
        public async Task<ActionResult> UpdateAvailability([FromBody] UpdatedAvailabilityDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var availability = await availabilityService.GetAvailabilityByIdAsync(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, availability, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var updated = await availabilityService.UpdateAvailabilityAsync(model);
            if (!updated)
                return BadRequest("Availability could be not updated");

            return NoContent();
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
                return Forbid();

            var deleted = await availabilityService.DeleteAvailabilityAsync(availabilityId);
            if (!deleted)
                return BadRequest("Availability could be not deleted");

            return NoContent();
        }
    }
}
