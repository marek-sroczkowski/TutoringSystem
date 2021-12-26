using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class RepeatedReservationRepository : RepositoryBase<RepeatedReservation>, IRepeatedReservationRepository
    {
        public RepeatedReservationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddReservationAsync(RepeatedReservation reservation)
        {
            Create(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteReservationAsync(RepeatedReservation reservation)
        {
            reservation.IsActive = false;

            return await UpdateReservationAsync(reservation);
        }

        public async Task<RepeatedReservation> GetReservationAsync(Expression<Func<RepeatedReservation, bool>> expression)
        {
            var reservation = await DbContext.RepeatedReservations
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(expression);

            return reservation;
        }

        public IEnumerable<RepeatedReservation> GetReservationsCollection(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));

            var reservations = FindByCondition(expression)
                .Include(r => r.Reservations);

            return reservations;
        }

        public async Task<bool> UpdateReservationAsync(RepeatedReservation updatedReservation)
        {
            Update(updatedReservation);

            return await SaveChangedAsync();
        }
    }
}