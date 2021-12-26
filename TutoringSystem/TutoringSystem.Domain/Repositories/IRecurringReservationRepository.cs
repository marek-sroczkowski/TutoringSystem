using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IRecurringReservationRepository
    {
        Task<bool> AddReservationAsync(RecurringReservation reservation);
        Task<bool> UpdateReservationAsync(RecurringReservation updatedReservation);
        Task<bool> DeleteReservationAsync(RecurringReservation reservation);
        Task<RecurringReservation> GetReservationAsync(Expression<Func<RecurringReservation, bool>> expression);
        Task<IEnumerable<RecurringReservation>> GetReservationsCollectionAsync(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true);
    }
}