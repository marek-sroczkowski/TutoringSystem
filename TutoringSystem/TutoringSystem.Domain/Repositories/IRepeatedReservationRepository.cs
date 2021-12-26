using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IRepeatedReservationRepository
    {
        Task<bool> AddReservationAsync(RepeatedReservation reservation);
        Task<bool> UpdateReservationAsync(RepeatedReservation updatedReservation);
        Task<bool> DeleteReservationAsync(RepeatedReservation reservation);
        Task<RepeatedReservation> GetReservationAsync(Expression<Func<RepeatedReservation, bool>> expression);
        IEnumerable<RepeatedReservation> GetReservationsCollection(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true);
    }
}