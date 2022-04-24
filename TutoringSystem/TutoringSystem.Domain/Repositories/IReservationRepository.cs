using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> GetReservationAsync(Expression<Func<Reservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<Reservation> GetReservationsCollection(Expression<Func<Reservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<Reservation>> GetReservationsCollectionAsync(Expression<Func<Reservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool ReservationExists(Expression<Func<Reservation, bool>> expression, bool? isActive = true);
        Task<bool> RemoveReservationAsync(Reservation reservation);
        Task<bool> UpdateReservationAsync(Reservation updatedReservation);
        Task<bool> UpdateReservationsCollectionAsync(IEnumerable<Reservation> reservations);
    }
}