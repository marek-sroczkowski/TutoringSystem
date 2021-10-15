using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class Student : User
    {
        public double HourlRate { get; set; }
        public string Note { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Tutor> Tutors { get; set; }

        public Student()
        {
            Role = Role.Student;
            IsEnable = true;
        }
    }
}
