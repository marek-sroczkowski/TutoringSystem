using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Application.Models.Dtos.Tutor;
using TutoringSystem.Application.Models.Parameters;

namespace TutoringSystem.API.Controllers
{
    [Route("api/tutor")]
    [ApiController]
    [Authorize]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService tutorService;

        public TutorController(ITutorService tutorService)
        {
            this.tutorService = tutorService;
        }

        [SwaggerOperation(Summary = "Retrieves tutors filtered by specific parameters")]
        [HttpGet("all")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<TutorSimpleDto>>> GetTutors([FromQuery] SearchedUserParameters parameters)
        {
            var tutors = await tutorService.GetTutors(parameters);
            Response.Headers.Add("X-Pagination", tutors.GetPaginationJsonMetadata());

            return Ok(tutors);
        }

        [SwaggerOperation(Summary = "Retrieves all tutors of the current logged in student")]
        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<TutorDto>>> GetTutorsByStudent()
        {
            var students = await tutorService.GetTutorsByStudentIdAsync(User.GetUserId());

            return Ok(students);
        }

        [SwaggerOperation(Summary = "Retrieves a specific tutor by unique id")]
        [HttpGet("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateTutorExistence]
        public async Task<ActionResult<TutorDetailsDto>> GetTutor(long tutorId)
        {
            var tutor = await tutorService.GetTutorAsync(tutorId, User.GetUserId());

            return Ok(tutor);
        }

        [SwaggerOperation(Summary = "Removes a student from the current logged in tutor's student list")]
        [HttpDelete("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateStudentExistence]
        public async Task<ActionResult> RemoveTutor(long tutorId)
        {
            var removed = await tutorService.RemoveTutorAsync(User.GetUserId(), tutorId);

            return removed ? NoContent() : BadRequest("Tutor could be not removed from student's tutor list");
        }
    }
}