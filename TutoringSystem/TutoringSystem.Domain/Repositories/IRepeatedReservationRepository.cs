using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IRepeatedReservationRepository
    {
        Task<bool> AddReservationAsync(RepeatedReservation reservation);
        Task<bool> AddReservationsCollectionAsync(IEnumerable<RepeatedReservation> reservations);
        Task<RepeatedReservation> GetReservationAsync(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<RepeatedReservation> GetReservationsCollection(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<RepeatedReservation>> GetReservationsCollectionAsync(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool IsReservationExist(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true);
        Task<bool> RemoveReservationAsync(RepeatedReservation reservation);
        Task<bool> UpdateReservationAsync(RepeatedReservation updatedReservation);
        Task<bool> UpdateReservationsCollectionAsync(IEnumerable<RepeatedReservation> reservations);
    }
}