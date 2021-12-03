using System;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class GeneralTimedReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TutorReportDto Report { get; set; }

        public GeneralTimedReportDto()
        {
        }

        public GeneralTimedReportDto(DateTime startDate, DateTime endDate, TutorReportDto report)
        {
            StartDate = startDate;
            EndDate = endDate;
            Report = report;
        }
    }
}
