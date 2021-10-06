using TutoringSystem.Application.Dtos.SubjectDtos;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class SubjectReportDto
    {
        public SubjectDto Subject { get; set; }
        public double TotalHours { get; set; }
        public double TotalProfit { get; set; }
    }
}
