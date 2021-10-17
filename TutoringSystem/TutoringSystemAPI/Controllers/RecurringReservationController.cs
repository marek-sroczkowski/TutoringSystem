using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.API.Filters.TypeFilters;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Parameters;
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
            var studentId = User.GetUserId();
            var resevations = await reservationService.GetReservationsByStudentAsync(studentId, parameters);

            var metadata = new
            {
                resevations.TotalCount,
                resevations.PageSize,
                resevations.CurrentPage,
                resevations.TotalPages,
                resevations.HasNext,
                resevations.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves all recurring reservations of the current logged in tutor filtered with selected parameters")]
        [HttpGet("tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<ReservationDto>>> GetTutorReservations([FromQuery] ReservationParameters parameters)
        {
            var tutorId = User.GetUserId();
            var resevations = await reservationService.GetReservationsByTutorAsync(tutorId, parameters);

            var metadata = new
            {
                resevations.TotalCount,
                resevations.PageSize,
                resevations.CurrentPage,
                resevations.TotalPages,
                resevations.HasNext,
                resevations.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

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
            if (!authorizationResult.Succeeded)
                return Forbid();

            return Ok(reservation);
        }

        [SwaggerOperation(Summary = "Creates a new student's recurring reservation")]
        [HttpPost("student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> CreateStudentReservation([FromBody] NewStudentRecurringReservationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var studentId = User.GetUserId();
            var reservation = await reservationService.AddReservationByStudentAsync(studentId, model);
            if (reservation is null)
                return BadRequest("New reservation could be not added");

            return Created("api/reservation/recurring/" + reservation.Id, null);
        }

        [SwaggerOperation(Summary = "Creates a new tutor's recurring reservation")]
        [HttpPost("tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> CreateTutorReservation([FromBody] NewTutorRecurringReservationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tutorId = User.GetUserId();
            var reservation = await reservationService.AddReservationByTutorAsync(tutorId, model);
            if (reservation is null)
                return BadRequest("New reservation could be not added");

            return Created("api/reservation/recurring/" + reservation.Id, null);
        }

        [SwaggerOperation(Summary = "Deletes a specific reservation")]
        [HttpDelete("{reservationId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateRecurringReservationExistence ]
        public async Task<ActionResult> DeleteReservation(long reservationId)
        {
            var reservation = await reservationService.GetReservationByIdAsync(reservationId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var deleted = await reservationService.DeleteReservationAsync(reservationId);
            if (!deleted)
                return BadRequest("Reservation could be not deleted");

            return NoContent();
        }
    }
}
