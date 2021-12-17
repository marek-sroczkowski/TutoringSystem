using System;
using System.Linq;
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
        IQueryable<Reservation> GetReservationsCollection(Expression<Func<Reservation, bool>> expression);
    }
}