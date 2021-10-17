using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class RecurringReservation : Reservation
    {
        public ReservationFrequency Frequency { get; set; }
        public long RepeatedReservationId { get; set; }
        public virtual RepeatedReservation Reservation { get; set; }
    }
}
