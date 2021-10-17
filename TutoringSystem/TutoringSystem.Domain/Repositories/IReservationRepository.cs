using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IReservationRepository
    {
        Task<bool> UpdateReservationAsync(Reservation updatedReservation);
        Task<bool> DeleteReservationAsync(Reservation reservation);
        Task<Reservation> GetReservationAsync(Expression<Func<Reservation, bool>> expression);
        Task<IEnumerable<Reservation>> GetReservationsCollectionAsync(Expression<Func<Reservation, bool>> expression);
    }
}
