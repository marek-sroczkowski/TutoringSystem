using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Report
{
    public class PlaceReportDto : BaseReportDto
    {
        public ReservationPlace Place { get; set; }
    }
}