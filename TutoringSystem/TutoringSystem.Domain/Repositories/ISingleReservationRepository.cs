using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ISingleReservationRepository
    {
        Task<bool> AddReservationAsync(SingleReservation reservation);
        Task<bool> UpdateReservationAsync(SingleReservation updatedReservation);
        Task<bool> DeleteReservationAsync(SingleReservation reservation);
        Task<SingleReservation> GetReservationAsync(Expression<Func<SingleReservation, bool>> expression);
        Task<IEnumerable<SingleReservation>> GetReservationsCollectionAsync(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true);
    }
}
