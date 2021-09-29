using System;

namespace TutoringSystem.Domain.Parameters
{
    public class FutureAvailabilityParameters : QueryStringParameters
    {
        public DateTime? EndDate { get; set; }
    }
}
