using System;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class Reservation
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public Place Place { get; set; }

        public virtual Lesson Lesson { get; set; }

        public long SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }

        public long StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
