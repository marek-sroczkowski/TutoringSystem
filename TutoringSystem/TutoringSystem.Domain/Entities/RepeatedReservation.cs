using System;
using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class RepeatedReservation
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }
        public DateTime NextAddedDate { get; set; }
        public ReservationFrequency Frequency { get; set; }

        public virtual ICollection<RecurringReservation> Reservations { get; set; }

        public RepeatedReservation()
        {
            CreationDate = DateTime.Now;
        }

        public RepeatedReservation(RecurringReservation reservation) : this()
        {
            LastAddedDate = reservation.StartTime;
            Reservations = new List<RecurringReservation> { reservation };
            Frequency = reservation.Frequency;
            SetNextSynchronizationDate();
        }

        private void SetNextSynchronizationDate()
        {
            switch (Frequency)
            {
                case ReservationFrequency.Weekly:
                    NextAddedDate = LastAddedDate.AddDays(7);
                    break;
                case ReservationFrequency.OnceTwoWeeks:
                    NextAddedDate = LastAddedDate.AddDays(14);
                    break;
                case ReservationFrequency.Monthly:
                    NextAddedDate = LastAddedDate.AddDays(28);
                    break;
            }
        }
    }
}