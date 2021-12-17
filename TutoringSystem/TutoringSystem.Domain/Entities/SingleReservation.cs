using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Domain.Entities
{
    public class SingleReservation : Reservation
    {
        public SingleReservation()
        {
            Type = ReservationType.Single;
        }
    }
}