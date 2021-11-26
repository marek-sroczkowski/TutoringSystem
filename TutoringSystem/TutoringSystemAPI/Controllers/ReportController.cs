﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.TypeFilters;
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

        [SwaggerOperation(Summary = "Retrieves report with statistics for the logged in tutor")]
        [HttpGet("summary")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<TutorReportDto>> GetGeneralReportAsync([FromQuery] ReportParameters parameters)
        {
            var report = await reportService.GetGeneralReportAsync(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all students for the logged in tutor")]
        [HttpGet("summary/students")]
        [Authorize(Roles = "Tutor")]
        [ValidateStudentExistence]
        public async Task<ActionResult<IEnumerable<TutorReportDto>>> GetStudentsReport([FromQuery] ReportParameters parameters)
        {
            var report = await reportService.GetStudentsReportAsync(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all subjects for the logged in tutor")]
        [HttpGet("summary/subjects")]
        [Authorize(Roles = "Tutor")]
        [ValidateSubjectExistence]
        public async Task<ActionResult<IEnumerable<TutorReportDto>>> GetSubjectsReport([FromQuery] ReportParameters parameters)
        {
            var report = await reportService.GetSubjectsReportAsync(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all subject categories for the logged in tutor")]
        [HttpGet("summary/subjects/category")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<TutorReportDto>> GetSubjectCategoryReport([FromQuery] ReportParameters parameters)
        {
            var report = reportService.GetSubjectCategoriesReport(User.GetUserId(), parameters);

            return Ok(report);
        }

        [SwaggerOperation(Summary = "Retrieves report with statistics about all places for the logged in tutor")]
        [HttpGet("summary/place")]
        [Authorize(Roles = "Tutor")]
        public ActionResult<IEnumerable<TutorReportDto>> GetPlaceReport([FromQuery] ReportParameters parameters)
        {
            var report = reportService.GetPlacesReport(User.GetUserId(), parameters);

            return Ok(report);
        }
    }
}