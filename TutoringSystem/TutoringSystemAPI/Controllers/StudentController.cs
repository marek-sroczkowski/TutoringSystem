using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.TypeFilters;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.Enums;

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

        [SwaggerOperation(Summary = "Retrieves all students of the current logged in tutor")]
        [HttpGet]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<StudentDto>>> GetStudents()
        {
            var students = await studentService.GetStudentsByTutorIdAsync(User.GetUserId());

            return Ok(students);
        }

        [SwaggerOperation(Summary = "Retrieves a specific student by unique id")]
        [HttpGet("{studentId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult<TutorDetailsDto>> GetStudent(long studentId)
        {
            var student = await studentService.GetStudentAsync(User.GetUserId(), studentId);

            return Ok(student);
        }

        [SwaggerOperation(Summary = "Adds a existing student to the students of the current logged in tutor")]
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult<AddStudentToTutorStatus>> AddStudent([FromBody] NewExistingStudentDto student)
        {
            var addedStatus = await studentService.AddStudentToTutorAsync(User.GetUserId(), student);

            return Ok(addedStatus);
        }

        [SwaggerOperation(Summary = "Removes a student from the current logged in tutor's student list")]
        [HttpDelete("{studentId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult> RemoveStudent(long studentId)
        {
            var removed = await studentService.RemoveStudentAsync(User.GetUserId(), studentId);
            if (!removed)
                return BadRequest("Student could be not removed from tutor's student list");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Removes all students from the current logged in tutor's student list")]
        [HttpDelete("all")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> RemoveAllStudent()
        {
            var removed = await studentService.RemoveAllStudentsAsync(User.GetUserId());
            if (!removed)
                return BadRequest("Student list could be not cleared");

            return NoContent();
        }
    }
}
