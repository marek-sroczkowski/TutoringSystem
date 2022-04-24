using System;

namespace TutoringSystem.Application.Models.Parameters
{
    public class FutureAvailabilityParameters : QueryStringParameters
    {
        public DateTime? EndDate { get; set; }
    }
}
