using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.API.Filters;
using TutoringSystem.Application.Dtos.StudentDtos;
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

        [SwaggerOperation(Summary = "Retrieves all students of the current logged in tutor")]
        [HttpGet("students")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<StudentDto>>> GetStudents()
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var students = await tutorService.GetStudentsAsync(long.Parse(tutorId));

            return Ok(students);
        }

        [SwaggerOperation(Summary = "Retrieves a specific tutor by unique id")]
        [HttpGet("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateTutorExistence]
        public async Task<ActionResult<TutorDetailsDto>> GetTutor(long tutorId)
        {
            var tutor = await tutorService.GetTutorAsync(tutorId);

            return Ok(tutor);
        }

        [SwaggerOperation(Summary = "Adds a specific student to the students of the current logged in tutor")]
        [HttpPost("{studentId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult> AddStudent(long studentId)
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var added = await tutorService.AddStudentAsync(long.Parse(tutorId), studentId);
            if (!added)
                return BadRequest("Student could not be added");

            return Ok();
        }

        [SwaggerOperation(Summary = "Removes a student from the current logged in tutor's student list")]
        [HttpDelete("{studentId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult> RemoveStudent(long studentId)
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var removed = await tutorService.RemoveStudentAsync(long.Parse(tutorId), studentId);
            if (!removed)
                return BadRequest("Student could be not removed from tutor's student list");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Removes all students from the current logged in tutor's student list")]
        [HttpDelete("all")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> RemoveAllStudent()
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var removed = await tutorService.RemoveAllStudentsAsync(long.Parse(tutorId));
            if (!removed)
                return BadRequest("Student list could be not cleared");

            return NoContent();
        }
    }
}
