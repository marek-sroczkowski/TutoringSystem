using System.Collections.Generic;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class TutorReportDto
    {
        public IEnumerable<StudentSummaryDto> StudentSummary { get; set; }
        public double TutoringProfit{ get; set; }
        public double OrderProfit { get; set; }
        public double TotalProfit { get; set; }
        public double TotalHours { get; set; }
    }
}
