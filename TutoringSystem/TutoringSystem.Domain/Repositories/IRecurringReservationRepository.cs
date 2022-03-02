using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IRecurringReservationRepository
    {
        Task<bool> AddReservationAsync(RecurringReservation reservation);
        Task<bool> AddReservationsCollectionAsync(IEnumerable<RecurringReservation> reservations);
        Task<RecurringReservation> GetReservationAsync(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<RecurringReservation> GetReservationsCollection(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<RecurringReservation>> GetReservationsCollectionAsync(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool IsReservationExist(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true);
        Task<bool> RemoveReservationAsync(RecurringReservation reservation);
        Task<bool> UpdateReservationAsync(RecurringReservation updatedReservation);
        Task<bool> UpdateReservationsCollectionAsync(IEnumerable<RecurringReservation> reservations);
    }
}