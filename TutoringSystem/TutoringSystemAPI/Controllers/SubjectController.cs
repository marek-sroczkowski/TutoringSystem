using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/subject")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService subjectService;
        private readonly IAuthorizationService authorizationService;

        public SubjectController(ISubjectService subjectService, IAuthorizationService authorizationService)
        {
            this.subjectService = subjectService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves all subjects of the current logged in tutor")]
        [HttpGet]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<List<SubjectDto>>> GetSubjects()
        {
            var subjects = await subjectService.GetTutorSubjectsAsync(User.GetUserId());

            return Ok(subjects);
        }

        [SwaggerOperation(Summary = "Retrieves all subjects of indicated by id tutor")]
        [HttpGet("student/{tutorId}")]
        [Authorize(Roles = "Student")]
        [ValidateTutorExistence]
        public async Task<ActionResult<List<SubjectDto>>> GetSubjects(long tutorId)
        {
            var subjects = await subjectService.GetTutorSubjectsAsync(tutorId);

            return Ok(subjects);
        }

        [SwaggerOperation(Summary = "Retrieves a specific subject by unique id")]
        [HttpGet("{subjectId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateSubjectExistence]
        public async Task<ActionResult<SubjectDetailsDto>> GetSubject(long subjectId)
        {
            var subject = await subjectService.GetSubjectByIdAsync(subjectId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, subject, new ResourceOperationRequirement(OperationType.Read)).Result;

            return authorizationResult.Succeeded ? Ok(subject) : Forbid();
        }

        [SwaggerOperation(Summary = "Creates a new subject")]
        [HttpPost]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> CreateSubject([FromBody] NewSubjectDto model)
        {
            var subject = await subjectService.AddSubjectAsync(User.GetUserId(), model);

            return subject != null
                ? Created("api/subject/" + subject.Id, null)
                : BadRequest("New subject could be not added");
        }

        [SwaggerOperation(Summary = "Updates a existing subject")]
        [HttpPut]
        [Authorize(Roles = "Tutor")]
        [ValidateSubjectExistence]
        public async Task<ActionResult> UpdateSubject([FromBody] UpdatedSubjectDto model)
        {
            var subject = await subjectService.GetSubjectByIdAsync(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, subject, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var updated = await subjectService.UpdateSubjectAsync(model);

            return updated ? NoContent() : BadRequest("Subject could be not updated");
        }

        [SwaggerOperation(Summary = "Deletes a specific subject")]
        [HttpDelete("{subjectId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateSubjectExistence]
        public async Task<ActionResult> DeleteSubject(long subjectId)
        {
            var subject = await subjectService.GetSubjectByIdAsync(subjectId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, subject, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            var deleted = await subjectService.DeactivateSubjectAsync(subjectId);

            return deleted ? NoContent() : BadRequest("Subject could be not deleted");
        }
    }
}