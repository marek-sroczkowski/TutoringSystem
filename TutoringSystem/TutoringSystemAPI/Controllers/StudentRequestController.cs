using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentRequestDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/student/request")]
    [ApiController]
    [Authorize]
    public class StudentRequestController : ControllerBase
    {
        private readonly IStudentRequestService requestService;
        private readonly IStudentTutorRequestNotificationService notificationService;

        public StudentRequestController(IStudentRequestService requestService,
            IStudentTutorRequestNotificationService notificationService)
        {
            this.requestService = requestService;
            this.notificationService = notificationService;
        }

        [SwaggerOperation(Summary = "Sends a request to tutor to include a specific student in the student list")]
        [HttpPost("{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateTutorExistence]
        public async Task<ActionResult<AddTutorToStudentStatus>> AddRequest(long tutorId)
        {
            var addedStatus = await requestService.AddRequestAsync(User.GetUserId(), tutorId);
            //if(addedStatus == AddTutorToStudentStatus.RequestCreated)
            //{
            //    await notificationService.SendNotificationToTutorDevice(User.GetUserId(), tutorId);
            //}

            return Ok(addedStatus);
        }

        [SwaggerOperation(Summary = "Declines a student's request to join a specific tutor")]
        [HttpPut("decline/{requestId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentRequestExistence]
        public async Task<ActionResult> DeclineRequest(long requestId)
        {
            var decline = await requestService.DeclineRequest(requestId);
            if (!decline)
                return BadRequest("Request could be not decline");

            return Ok();
        }

        [SwaggerOperation(Summary = "Retrieves all requests for the currently logged in student")]
        [HttpGet("byStudent")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<StudentRequestDto>>> GetRequestsByStudent()
        {
            var requests = await requestService.GetRequestsByStudentId(User.GetUserId());

            return Ok(requests);
        }

        [SwaggerOperation(Summary = "Retrieves all requests to join the currently logged-on tutor's student list")]
        [HttpGet("byTutor")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<StudentRequestDto>>> GetRequestsByTutor()
        {
            var requests = await requestService.GetRequestsByTutorId(User.GetUserId());

            return Ok(requests);
        }
    }
}