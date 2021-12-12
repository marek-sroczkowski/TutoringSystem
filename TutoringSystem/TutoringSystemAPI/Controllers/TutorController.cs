using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.TypeFilters;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Services.Interfaces;
using Newtonsoft.Json;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Dtos.Enums;

namespace TutoringSystem.API.Controllers
{
    [Route("api/tutor")]
    [ApiController]
    [Authorize]
    public class TutorController : ControllerBase
    {
        private readonly ITutorService tutorService;
        private readonly IStudentTutorRequestNotificationService notificationService;

        public TutorController(ITutorService tutorService,
            IStudentTutorRequestNotificationService notificationService)
        {
            this.tutorService = tutorService;
            this.notificationService = notificationService;
        }

        [SwaggerOperation(Summary = "Retrieves tutors filtered by specific parameters")]
        [HttpGet("all")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<TutorSimpleDto>>> GetTutors([FromQuery] SearchedTutorParameters parameters)
        {
            var tutors = await tutorService.GetTutors(parameters);

            var metadata = new
            {
                tutors.TotalCount,
                tutors.PageSize,
                tutors.CurrentPage,
                tutors.TotalPages,
                tutors.HasNext,
                tutors.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

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

        [SwaggerOperation(Summary = "Adds a specific tutor to the tutors of the current logged in student")]
        [HttpPost("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateTutorExistence]
        public async Task<ActionResult<AddTutorToStudentStatus>> AddTutor(long tutorId)
        {
            var addedStatus = await tutorService.AddTutorToStudentAsync(User.GetUserId(), tutorId);
            //if(addedStatus == AddTutorToStudentStatus.RequestCreated)
            //{
            //    await notificationService.SendNotificationToTutorDevice(User.GetUserId(), tutorId);
            //}

            return Ok(addedStatus);
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
