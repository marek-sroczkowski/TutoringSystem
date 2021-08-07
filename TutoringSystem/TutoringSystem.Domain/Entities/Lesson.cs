namespace TutoringSystem.Domain.Entities
{
    public class Lesson
    {
        public long Id { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }

        public long ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
