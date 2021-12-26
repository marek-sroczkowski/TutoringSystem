using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/reservation/repeated")]
    [ApiController]
    [Authorize]
    public class RepeatedReservationConstroller : ControllerBase
    {
        private readonly IRepeatedReservationService reservationService;

        public RepeatedReservationConstroller(IRepeatedReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [SwaggerOperation(Summary = "Retrieves all repeated reservations of the current logged in student filtered with selected parameters")]
        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public ActionResult<IEnumerable<RepeatedReservationDto>> GetStudentReservations()
        {
            var resevations = reservationService.GetReservationsByStudent(User.GetUserId());

            return Ok(resevations);
        }

        [SwaggerOperation(Summary = "Retrieves all repeated reservations of the current logged in tutor filtered with selected parameters")]
        [HttpGet("tutor")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<RepeatedReservationDto>> GetTutorReservations()
        {
            var resevations = reservationService.GetReservationsByTutor(User.GetUserId());

            return Ok(resevations);
        }
    }
}