using System;
using TutoringSystem.Domain.Entities.Base;

namespace TutoringSystem.Domain.Entities
{
    public class Interval : Entity
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public long AvailabilityId { get; set; }
        public virtual Availability Availability { get; set; }
    }
}
