using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.API.Filters;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/report")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;
        private readonly ISubjectService subjectService;
        private readonly IStudentService studentService;
        private readonly IAuthorizationService authorizationService;

        public ReportController(IReportService reportService, ISubjectService subjectService, IStudentService studentService, IAuthorizationService authorizationService)
        {
            this.reportService = reportService;
            this.subjectService = subjectService;
            this.studentService = studentService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics for the logged in tutor")]
        [HttpGet("summary")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<TutorReportDto>> GetReportAsync([FromQuery] ReportParameters parameters)
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var report = await reportService.GetReportByTutorAsync(long.Parse(tutorId), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about specific student for the logged in tutor")]
        [HttpGet("summary/student/{studentId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult<TutorReportDto>> GetStudentReport(long studentId, [FromQuery] ReportParameters parameters)
        {
            var student = await studentService.GetStudentAsync(studentId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, student, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var report = await reportService.GetStudentSummaryAsync(studentId, parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about specific subject for the logged in tutor")]
        [HttpGet("summary/subject/{subjectId}")]
        [Authorize(Roles = "Tutor")]
        [ValidateSubjectExistence]
        public async Task<ActionResult<TutorReportDto>> GetSubjectReport(long subjectId, [FromQuery] ReportParameters parameters)
        {
            var subject = await subjectService.GetSubjectByIdAsync(subjectId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, subject, new ResourceOperationRequirement(OperationType.Read)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var report = await reportService.GetSubjectReportAsync(long.Parse(tutorId), subjectId, parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about specific subject category for the logged in tutor")]
        [HttpGet("summary/subjectCategory")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<TutorReportDto>> GetSubjectCategoryReport([FromQuery] ReportSubjectCategoryParameters parameters)
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var report = await reportService.GetSubjectCategoryReportAsync(long.Parse(tutorId), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about specific place for the logged in tutor")]
        [HttpGet("summary/place")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<TutorReportDto>> GetPlaceReport([FromQuery] ReportPlaceParameters parameters)
        {
            var tutorId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var report = await reportService.GetPlaceReportAsync(long.Parse(tutorId), parameters);

            return Ok(report);
        }
    }
}
