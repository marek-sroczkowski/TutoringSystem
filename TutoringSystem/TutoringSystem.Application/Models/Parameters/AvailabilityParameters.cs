using System;

namespace TutoringSystem.Application.Models.Parameters
{
    public class AvailabilityParameters : QueryStringParameters
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
