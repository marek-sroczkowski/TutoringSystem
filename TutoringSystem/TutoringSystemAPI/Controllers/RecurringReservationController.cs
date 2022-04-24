using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Reservation;
using TutoringSystem.Application.Models.Enums;
using TutoringSystem.Application.Models.Parameters;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/reservation/recurring")]
    [ApiController]
    [Authorize]
    public class RecurringReservationController : ControllerBase
    {
        private readonly IRecurringReservationService reservationService;
        private readonly IAuthorizationService authorizationService;

        public RecurringReservationController(IRecurringReservationService reservationService, IAuthorizationService authorizationService)
        {
            this.reservationService = reservationService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves all recurring reservations of the current logged in student filtered with selected parameters")]
        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<ReservationDto>>> GetStudentReservations([FromQuery] ReservationParameters parameters)
        {
            var resevations = await reservationService.GetReservationsByStudentAsync(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", resevations.GetPaginationJsonMetadata());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves all recurring reservations of the current logged in tutor filtered with selected parameters")]
        [HttpGet("tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<ReservationDto>>> GetTutorReservations([FromQuery] ReservationParameters parameters)
        {
            var resevations = await reservationService.GetReservationsByTutorAsync(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", resevations.GetPaginationJsonMetadata());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves a specific recurring reservation by unique id")]
        [HttpGet("{reservationId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateRecurringReservationExistence]
        public async Task<ActionResult<ReservationDetailsDto>> GetReservation(long reservationId)
        {
            var reservation = await reservationService.GetReservationByIdAsync(reservationId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Read)).Result;

            return authorizationResult.Succeeded ? Ok(reservation) : Forbid();
        }

        [SwaggerOperation(Summary = "Creates a new student's recurring reservation")]
        [HttpPost("student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> CreateStudentReservation([FromBody] NewStudentRecurringReservationDto model)
        {
            var reservation = await reservationService.AddReservationByStudentAsync(User.GetUserId(), model);

            return reservation != null
                ? Created("api/reservation/recurring/" + reservation.Id, null)
                : BadRequest("New reservation could be not added");
        }

        [SwaggerOperation(Summary = "Creates a new tutor's recurring reservation")]
        [HttpPost("tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> CreateTutorReservation([FromBody] NewTutorRecurringReservationDto model)
        {
            var reservation = await reservationService.AddReservationByTutorAsync(User.GetUserId(), model);

            return reservation != null
                ? Created("api/reservation/recurring/" + reservation.Id, null)
                : BadRequest("New reservation could be not added");
        }

        [SwaggerOperation(Summary = "Deletes a specific reservation")]
        [HttpDelete("{reservationId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateRecurringReservationExistence ]
        public async Task<ActionResult> DeleteReservation(long reservationId, [FromQuery] RecurringReservationRemovingMode mode)
        {
            var reservation = await reservationService.GetReservationByIdAsync(reservationId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var deleted = await reservationService.RemoveReservationAsync(reservationId, mode);

            return deleted ? NoContent() : BadRequest("Reservation could be not deleted");
        }
    }
}
