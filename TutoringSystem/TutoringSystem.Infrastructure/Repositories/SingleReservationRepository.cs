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
    public class SingleReservationRepository : RepositoryBase<SingleReservation>, ISingleReservationRepository
    {
        public SingleReservationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddReservationAsync(SingleReservation reservation)
        {
            Create(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteReservationAsync(SingleReservation reservation)
        {
            Delete(reservation);

            return await SaveChangedAsync();
        }

        public async Task<SingleReservation> GetReservationAsync(Expression<Func<SingleReservation, bool>> expression)
        {
            var reservation = await DbContext.SingleReservations
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(expression);

            return reservation;
        }

        public async Task<IEnumerable<SingleReservation>> GetReservationsCollectionAsync(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));

            var reservations = await FindByCondition(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .ToListAsync();

            return reservations;
        }

        public async Task<bool> UpdateReservationAsync(SingleReservation updatedReservation)
        {
            Update(updatedReservation);

            return await SaveChangedAsync();
        }
    }
}
