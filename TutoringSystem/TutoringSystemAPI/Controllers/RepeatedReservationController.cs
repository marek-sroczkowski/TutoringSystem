using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Reservation;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/reservation/repeated")]
    [ApiController]
    [Authorize]
    public class RepeatedReservationController : ControllerBase
    {
        private readonly IRepeatedReservationService reservationService;

        public RepeatedReservationController(IRepeatedReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [SwaggerOperation(Summary = "Retrieves all repeated reservations of the current logged in student filtered with selected parameters")]
        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<RepeatedReservationDto>>> GetStudentReservations()
        {
            var resevations = await reservationService.GetReservationsByStudent(User.GetUserId());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves all repeated reservations of the current logged in tutor filtered with selected parameters")]
        [HttpGet("tutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<RepeatedReservationDto>>> GetTutorReservations()
        {
            var resevations = await reservationService.GetReservationsByTutor(User.GetUserId());

            return Ok(resevations);
        }
    }
}