using System;
using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class RepeatedReservation
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }
        public DateTime NextAddedDate { get; set; }
        public ReservationFrequency Frequency { get; set; }
        public bool IsActive { get; set; }

        public long StudentId { get; set; }
        public long TutorId { get; set; }

        public virtual ICollection<RecurringReservation> Reservations { get; set; }

        public RepeatedReservation()
        {
            CreationDate = DateTime.Now;
            IsActive = true;
        }

        public RepeatedReservation(RecurringReservation reservation) : this()
        {
            LastAddedDate = DateTime.Now;
            StartTime = reservation.StartTime;
            Duration = reservation.Duration;
            StudentId = reservation.StudentId;
            TutorId = reservation.TutorId;

            Reservations = new List<RecurringReservation> { reservation };
            Frequency = reservation.Frequency;
            SetNextSynchronizationDate();
        }

        private void SetNextSynchronizationDate()
        {
            NextAddedDate = Frequency switch
            {
                ReservationFrequency.Weekly => LastAddedDate.AddDays(7),
                ReservationFrequency.OnceTwoWeeks => LastAddedDate.AddDays(14),
                ReservationFrequency.Monthly => LastAddedDate.AddDays(28),
                _ => LastAddedDate.AddDays(7)
            };
        }
    }
}