using System;

namespace TutoringSystem.Domain.Entities
{
    public class Interval
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public long AvailabilityId { get; set; }
        public virtual Availability Availability { get; set; }
    }
}
