using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Base;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class Subject : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public SubjectPlace Place { get; set; }
        public SubjectCategory Category { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public long TutorId { get; set; }
        public virtual Tutor Tutor { get; set; }

        public Subject()
        {
            IsActive = true;
        }
    }
}
