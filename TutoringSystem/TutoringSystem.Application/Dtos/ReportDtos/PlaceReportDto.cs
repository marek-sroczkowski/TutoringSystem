using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReportDtos
{
    public class PlaceReportDto
    {
        public ReservationPlace Place { get; set; }
        public double TotalHours { get; set; }
        public double TotalProfit { get; set; }
    }
}
