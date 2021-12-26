using System;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class Reservation
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public bool IsPaid { get; set; }
        public ReservationPlace Place { get; set; }
        public ReservationType Type { get; set; }
        public bool IsActive { get; set; }

        public long SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }

        public long StudentId { get; set; }
        public virtual Student Student { get; set; }

        public Reservation()
        {
            IsActive = true;
        }
    }
}
