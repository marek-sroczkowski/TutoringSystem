using System;
using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Base;

namespace TutoringSystem.Domain.Entities
{
    public class Availability : Entity
    {
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public virtual ICollection<Interval> Intervals { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }
    }
}
