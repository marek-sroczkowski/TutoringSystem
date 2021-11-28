using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class SubjectCategoryReportDto : BaseReportDto
    {
        public SubjectCategory SubjectCategory { get; set; }
    }
}
