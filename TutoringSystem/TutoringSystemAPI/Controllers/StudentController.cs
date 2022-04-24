using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Services.Interfaces;
using Newtonsoft.Json;
using TutoringSystem.Application.Models.Dtos.Student;
using TutoringSystem.Application.Models.Dtos.Tutor;
using TutoringSystem.Application.Models.Enums;
using TutoringSystem.Application.Models.Parameters;

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

        [SwaggerOperation(Summary = "Retrieves students filtered by specific parameters")]
        [HttpGet("all")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<StudentSimpleDto>>> GetTutors([FromQuery] SearchedUserParameters parameters)
        {
            var students = await studentService.GetStudents(parameters);
            Response.Headers.Add("X-Pagination", students.GetPaginationJsonMetadata());

            return Ok(students);
        }

        [SwaggerOperation(Summary = "Retrieves all students of the current logged in tutor")]
        [HttpGet]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
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

            return removed ? NoContent() : BadRequest("Student could be not removed from tutor's student list");
        }

        [SwaggerOperation(Summary = "Updates student data related to a specific tutor")]
        [HttpPut]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> UpdateStudent([FromBody] UpdatedStudentDto student)
        {
            var updated = await studentService.UpdateStudentAsync(User.GetUserId(), student);

            return updated ? NoContent() : BadRequest("Student could not be updated");
        }
    }
}