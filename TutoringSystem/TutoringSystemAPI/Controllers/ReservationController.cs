using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/reservation")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService reservationService;
        private readonly IAuthorizationService authorizationService;

        public ReservationController(IReservationService reservationService,
            IAuthorizationService authorizationService)
        {
            this.reservationService = reservationService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves all reservations of the current logged in student filtered with selected parameters")]
        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public ActionResult<IEnumerable<ReservationDto>> GetStudentReservations([FromQuery] ReservationParameters parameters)
        {
            var resevations = reservationService.GetReservationsByStudent(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", resevations.GetPaginationJsonMetadata());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves all reservations of the current logged in tutor filtered with selected parameters")]
        [HttpGet("tutor")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<ReservationDto>> GetTutorReservations([FromQuery] ReservationParameters parameters)
        {
            var resevations = reservationService.GetReservationsByTutor(User.GetUserId(), parameters);
            Response.Headers.Add("X-Pagination", resevations.GetPaginationJsonMetadata());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves a specific reservation by unique id")]
        [HttpGet("{reservationId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateReservationExistence]
        public async Task<ActionResult<ReservationDetailsDto>> GetReservation(long reservationId)
        {
            var reservation = await reservationService.GetReservationByIdAsync(reservationId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, reservation, new ResourceOperationRequirement(OperationType.Read)).Result;

            return authorizationResult.Succeeded ? Ok(reservation) : Forbid();
        }
    }
}