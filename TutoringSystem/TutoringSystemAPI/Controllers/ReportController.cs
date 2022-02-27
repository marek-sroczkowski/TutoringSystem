using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using System.Collections.Generic;

namespace TutoringSystem.API.Controllers
{
    [Route("api/report")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService reportService;

        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [SwaggerOperation(Summary = "Retrieves a general statistics report with a monthly time stamp")]
        [HttpGet("summary/timed")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<GeneralTimedReportDto>> GetGeneralTimedReport([FromQuery] ReportParameters parameters)
        {
            var report = reportService.GetGeneralTimedReport(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics for the logged in tutor")]
        [HttpGet("summary")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<TutorReportDto> GetGeneralReportAsync([FromQuery] ReportParameters parameters)
        {
            var report = reportService.GetGeneralReport(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all students for the logged in tutor")]
        [HttpGet("summary/students")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult<IEnumerable<StudentReportDto>>> GetStudentsReport([FromQuery] ReportParameters parameters)
        {
            var report = await reportService.GetStudentsReportAsync(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all subjects for the logged in tutor")]
        [HttpGet("summary/subjects")]
        [Authorize(Roles = "Tutor")]
        [ValidateSubjectExistence]
        public async Task<ActionResult<IEnumerable<SubjectReportDto>>> GetSubjectsReport([FromQuery] ReportParameters parameters)
        {
            var report = await reportService.GetSubjectsReportAsync(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all subject categories for the logged in tutor")]
        [HttpGet("summary/subjects/categories")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<SubjectCategoryReportDto>> GetSubjectCategoryReport([FromQuery] ReportParameters parameters)
        {
            var report = reportService.GetSubjectCategoriesReport(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all places for the logged in tutor")]
        [HttpGet("summary/places")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<PlaceReportDto>> GetPlaceReport([FromQuery] ReportParameters parameters)
        {
            var report = reportService.GetPlacesReport(User.GetUserId(), parameters);

            return Ok(report);
        }
    }
}