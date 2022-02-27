using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ISingleReservationRepository
    {
        Task<bool> AddReservationAsync(SingleReservation reservation);
        Task<bool> AddReservationsCollectionAsync(IEnumerable<SingleReservation> reserservations);
        Task<SingleReservation> GetReservationAsync(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<SingleReservation> GetReservationsCollection(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<SingleReservation>> GetReservationsCollectionAsync(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool IsReservationExist(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true);
        Task<bool> RemoveReservationAsync(SingleReservation reservation);
        Task<bool> UpdateReservationAsync(SingleReservation updatedReservation);
        Task<bool> UpdateReservationsCollectionAsync(IEnumerable<SingleReservation> reservations);
    }
}