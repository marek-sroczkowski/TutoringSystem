using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IReservationRepository
    {
        Task<bool> AddReservationAsync(Reservation reservation);
        Task<Reservation> GetReservationByIdAsync(long reservationId);
        Task<bool> UpdateReservationAsync(Reservation updatedReservation);
        Task<bool> DeleteReservationAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetReservationsAsync(Expression<Func<Reservation, bool>> expression);
    }
}
