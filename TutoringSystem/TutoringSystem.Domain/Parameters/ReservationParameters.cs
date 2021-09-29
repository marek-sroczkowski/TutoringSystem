using System;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Parameters
{
    public class ReservationParameters : QueryStringParameters
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ReservationPlace? Place { get; set; }
    }
}
