using System;

namespace TutoringSystem.Application.Parameters
{
    public class FutureAvailabilityParameters : QueryStringParameters
    {
        public DateTime? EndDate { get; set; }
    }
}
