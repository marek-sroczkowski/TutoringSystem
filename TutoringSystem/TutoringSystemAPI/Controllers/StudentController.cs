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
    [Route("api/student")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [SwaggerOperation(Summary = "Retrieves all tutors of the current logged in student")]
        [HttpGet("tutors")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<TutorDto>>> GetTutors()
        {
            var studentId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var students = await studentService.GetTutorsAsync(long.Parse(studentId));

            return Ok(students);
        }

        [SwaggerOperation(Summary = "Retrieves a specific student by unique id")]
        [HttpGet("{studentId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult<TutorDetailsDto>> GetStudent(long studentId)
        {
            var student = await studentService.GetStudentAsync(studentId);

            return Ok(student);
        }

        [SwaggerOperation(Summary = "Adds a specific tutor to the tutors of the current logged in student")]
        [HttpPost("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateTutorExistence]
        public async Task<ActionResult> AddTutor(long tutorId)
        {
            var studentId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var added = await studentService.AddTutorAsync(long.Parse(studentId), tutorId);
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
            var studentId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var removed = await studentService.RemoveTutorAsync(long.Parse(studentId), tutorId);
            if (!removed)
                return BadRequest("Tutor could be not removed from student's tutor list");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Removes all tutors from the current logged in student student's list")]
        [HttpDelete("all")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> RemoveAllTutors()
        {
            var studentId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var removed = await studentService.RemoveAllTutorsAsync(long.Parse(studentId));
            if (!removed)
                return BadRequest("Tutor list could be not cleared");

            return NoContent();
        }
    }
}
