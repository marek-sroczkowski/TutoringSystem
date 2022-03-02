using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Application.Parameters;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Extensions;

namespace TutoringSystem.API.Controllers
{
    [Route("api/reservation/single")]
    [ApiController]
    [Authorize]
    public class SingleReservationController : ControllerBase
    {
        private readonly ISingleReservationService reservationService;
        private readonly IAuthorizationService authorizationService;

        public SingleReservationController(ISingleReservationService reservationService, IAuthorizationService authorizationService)
        {
            this.reservationService = reservationService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves all reservations of the current logged in student filtered with selected parameters")]
        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<ReservationDto>>> GetStudentReservations([FromQuery] ReservationParameters parameters)
        {
            var resevations = await reservationService.GetReservationsByStudentAsync(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", resevations.GetPaginationJsonMetadata());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves all reservations of the current logged in tutor filtered with selected parameters")]
        [HttpGet("tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<ReservationDto>>> GetTutorReservations([FromQuery] ReservationParameters parameters)
        {
            var resevations = await reservationService.GetReservationsByTutorAsync(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", resevations.GetPaginationJsonMetadata());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves a specific reservation by unique id")]
        [HttpGet("{reservationId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateSingleReservationExistence]
        public async Task<ActionResult<ReservationDetailsDto>> GetReservation(long reservationId)
        {
            var reservation = await reservationService.GetReservationByIdAsync(reservationId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Read)).Result;

            return authorizationResult.Succeeded ? Ok(reservation) : Forbid();
        }

        [SwaggerOperation(Summary = "Creates a new student's reservation")]
        [HttpPost("student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> CreateStudentReservation([FromBody] NewStudentSingleReservationDto model)
        {
            var reservation = await reservationService.AddReservationByStudentAsync(User.GetUserId(), model);

            return reservation != null
                ? Created("api/reservation/single/" + reservation.Id, null)
                : BadRequest("New reservation could be not added");
        }

        [SwaggerOperation(Summary = "Creates a new tutor's reservation")]
        [HttpPost("tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> CreateTutorReservation([FromBody] NewTutorSingleReservationDto model)
        {
            var reservation = await reservationService.AddReservationByTutorAsync(User.GetUserId(), model);

            return reservation != null
                ? Created("api/reservation/single/" + reservation.Id, null)
                : BadRequest("New reservation could be not added");
        }

        [SwaggerOperation(Summary = "Updates a existing reservation by tutor")]
        [HttpPut("tutor")]
        [Authorize(Roles = "Tutor")]
        [ValidateSingleReservationExistence]
        public async Task<ActionResult> UpdateReservationByTutor([FromBody] UpdatedTutorReservationDto model)
        {
            var reservation = await reservationService.GetReservationByIdAsync(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var updated = await reservationService.UpdateTutorReservationAsync(model);

            return updated ? NoContent() : BadRequest("Reservation could be not updated");
        }

        [SwaggerOperation(Summary = "Updates a existing reservation by student")]
        [HttpPut("student")]
        [Authorize(Roles = "Student")]
        [ValidateSingleReservationExistence]
        public async Task<ActionResult> UpdateReservationByStudent([FromBody] UpdatedStudentReservationDto model)
        {
            var reservation = await reservationService.GetReservationByIdAsync(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var updated = await reservationService.UpdateStudentReservationAsync(model);

            return updated ? NoContent() : BadRequest("Reservation could be not updated");
        }

        [SwaggerOperation(Summary = "Deletes a specific reservation")]
        [HttpDelete("{reservationId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateSingleReservationExistence]
        public async Task<ActionResult> DeleteReservation(long reservationId)
        {
            var reservation = await reservationService.GetReservationByIdAsync(reservationId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var deleted = await reservationService.RemoveReservationAsync(reservationId);

            return deleted ? NoContent() : BadRequest("Reservation could be not deleted");
        }
    }
}