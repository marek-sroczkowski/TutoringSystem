﻿using System.Collections.Generic;

namespace TutoringSystem.Domain.Entities
{
    public class Student : User
    {
        public double HourlRate { get; set; }
        public virtual School School { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Tutor> Tutors { get; set; }
    }
}