using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Reservation> GetReservationAsync(Expression<Func<Reservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var reservation = isEagerLoadingEnabled
                ? await GetReservationWithEagerLoadingAsync(expression)
                : await GetReservationWithoutEagerLoadingAsync(expression);

            return reservation;
        }

        public IQueryable<Reservation> GetReservationsCollection(Expression<Func<Reservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var reservations = isEagerLoadingEnabled
                ? GetReservationsCollectionWithEagerLoading(expression)
                : Find(expression);

            return reservations;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsCollectionAsync(Expression<Func<Reservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var reservations = isEagerLoadingEnabled
                ? GetReservationsCollectionWithEagerLoading(expression)
                : Find(expression);

            return await reservations.ToListAsync();
        }

        public bool IsReservationExist(Expression<Func<Reservation, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveReservationAsync(Reservation reservation)
        {
            Delete(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateReservationAsync(Reservation reservation)
        {
            Update(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateReservationsCollectionAsync(IEnumerable<Reservation> reservations)
        {
            UpdateRange(reservations);

            return await SaveChangedAsync();
        }

        private async Task<Reservation> GetReservationWithEagerLoadingAsync(Expression<Func<Reservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private async Task<Reservation> GetReservationWithoutEagerLoadingAsync(Expression<Func<Reservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private IQueryable<Reservation> GetReservationsCollectionWithEagerLoading(Expression<Func<Reservation, bool>> expression)
        {
            var reservations = Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor);

            return reservations;
        }
    }
}