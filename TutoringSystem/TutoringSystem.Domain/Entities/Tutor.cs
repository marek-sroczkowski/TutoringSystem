using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class Tutor : User
    {
        public virtual ICollection<AdditionalOrder> AdditionalOrders { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public Tutor()
        {
            Role = Role.Tutor;
        }
    }
}
