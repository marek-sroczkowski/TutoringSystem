using System;
using System.Collections.Generic;

namespace TutoringSystem.Domain.Entities
{
    public class Availability
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }

        public virtual ICollection<Interval> Intervals { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }
    }
}
