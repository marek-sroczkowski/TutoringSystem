using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.TypeFilters;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Services.Interfaces;

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

        [SwaggerOperation(Summary = "Retrieves all tutors of the current logged in student")]
        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<TutorDto>>> GetTutors()
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
            var tutor = await tutorService.GetTutorByIdAsync(tutorId);

            return Ok(tutor);
        }

        [SwaggerOperation(Summary = "Adds a specific tutor to the tutors of the current logged in student")]
        [HttpPost("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateTutorExistence]
        public async Task<ActionResult> AddTutor(long tutorId)
        {
            var added = await tutorService.AddTutorToStudentAsync(User.GetUserId(), tutorId);
            if (!added)
                return BadRequest("Tutor could not be added");

            return Ok();
        }

        [SwaggerOperation(Summary = "Removes a student from the current logged in tutor's student list")]
        [HttpDelete("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateStudentExistence]
        public async Task<ActionResult> RemoveTutor(long tutorId)
        {
            var removed = await tutorService.RemoveTutorAsync(User.GetUserId(), tutorId);
            if (!removed)
                return BadRequest("Tutor could be not removed from student's tutor list");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Removes all tutors from the current logged in student student's list")]
        [HttpDelete("all")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> RemoveAllTutors()
        {
            var removed = await tutorService.RemoveAllTutorsAsync(User.GetUserId());
            if (!removed)
                return BadRequest("Tutor list could be not cleared");

            return NoContent();
        }
    }
}
