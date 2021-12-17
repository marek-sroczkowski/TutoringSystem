using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
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

        public ReservationController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
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
    }
}