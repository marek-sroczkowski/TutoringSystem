using System;
using System.Collections.Generic;

namespace TutoringSystem.Domain.Entities
{
    public class RepeatedReservation
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }

        public virtual ICollection<RecurringReservation> Reservations { get; set; }

        public RepeatedReservation()
        {
            CreationDate = DateTime.Now;
            LastAddedDate = CreationDate;
        }

        public RepeatedReservation(RecurringReservation reservation) : this()
        {
            Reservations = new List<RecurringReservation> { reservation };
        }
    }
}
