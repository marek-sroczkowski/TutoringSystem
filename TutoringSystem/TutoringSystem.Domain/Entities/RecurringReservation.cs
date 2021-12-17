using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class RecurringReservation : Reservation
    {
        public ReservationFrequency Frequency { get; set; }
        public long RepeatedReservationId { get; set; }
        public virtual RepeatedReservation Reservation { get; set; }

        public RecurringReservation()
        {
            Type = ReservationType.Recurring;
        }

        public RecurringReservation(RecurringReservation reservation) : this()
        {
            Cost = reservation.Cost;
            Duration = reservation.Duration;
            Description = reservation.Description;
            Place = reservation.Place;
            SubjectId = reservation.SubjectId;
            TutorId = reservation.TutorId;
            StudentId = reservation.StudentId;
            Frequency = reservation.Frequency;
            RepeatedReservationId = reservation.RepeatedReservationId;
        }
    }
}