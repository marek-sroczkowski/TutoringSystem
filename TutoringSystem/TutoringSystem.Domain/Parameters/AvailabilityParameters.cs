using System;

namespace TutoringSystem.Domain.Parameters
{
    public class AvailabilityParameters : QueryStringParameters
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
