using System.Collections.Generic;

namespace TutoringSystem.Domain.Entities
{
    public class Tutor : User
    {
        public virtual ICollection<AdditionalOrder> AdditionalOrders { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
