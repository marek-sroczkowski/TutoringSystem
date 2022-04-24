using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Report
{
    public class SubjectCategoryReportDto : BaseReportDto
    {
        public SubjectCategory SubjectCategory { get; set; }
    }
}
