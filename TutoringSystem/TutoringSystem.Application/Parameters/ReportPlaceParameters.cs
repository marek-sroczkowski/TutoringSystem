using System;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Parameters
{
    public class ReportPlaceParameters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReservationPlace Place { get; set; }
    }
}
