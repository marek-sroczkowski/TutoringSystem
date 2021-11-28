using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class PlaceReportDto : BaseReportDto
    {
        public ReservationPlace Place { get; set; }
    }
}