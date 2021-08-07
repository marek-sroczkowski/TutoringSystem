using System.Collections.Generic;

namespace TutoringSystem.Domain.Entities
{
    public class Subject
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }
    }
}
