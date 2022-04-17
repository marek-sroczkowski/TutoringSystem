using System;
using System.Collections.Generic;
using TutoringSystem.Domain.Entities.Base;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Extensions;

namespace TutoringSystem.Domain.Entities
{
    public class RepeatedReservation : Entity
    {
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public ReservationPlace Place { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }
        public DateTime NextAddedDate { get; set; }
        public ReservationFrequency Frequency { get; set; }
        public bool IsActive { get; set; }

        public long StudentId { get; set; }
        public long TutorId { get; set; }
        public long SubjectId { get; set; }

        public virtual ICollection<RecurringReservation> Reservations { get; set; }

        public RepeatedReservation()
        {
            CreationDate = DateTime.Now.ToLocal();
            IsActive = true;
        }

        public RepeatedReservation(RecurringReservation reservation) : this()
        {
            LastAddedDate = DateTime.Now.ToLocal();
            StartTime = reservation.StartTime;
            Duration = reservation.Duration;
            Place = reservation.Place;
            StudentId = reservation.StudentId;
            TutorId = reservation.TutorId;
            SubjectId = reservation.SubjectId;
            Frequency = reservation.Frequency;

            Reservations = new List<RecurringReservation> { reservation };
            AddReservations(reservation);
            SetNextSynchronizationDate();
        }

        private void AddReservations(RecurringReservation reservation)
        {
            var reservationDate = reservation.StartTime.Date;
            int i = 1;
            while (reservationDate.AddDays((int)Frequency * i) <= DateTime.Now.ToLocal().Date)
            {
                Reservations.Add(new RecurringReservation(reservation)
                {
                    StartTime = reservation.StartTime.AddDays((int)Frequency * i)
                });
                i++;
            }
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