using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class SubjectCategoryReportDto
    {
        public SubjectCategory SubjectCategory { get; set; }
        public double TotalHours { get; set; }
        public double TotalProfit { get; set; }
    }
}
