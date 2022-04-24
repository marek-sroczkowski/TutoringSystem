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
    public class RecurringReservationRepository : RepositoryBase<RecurringReservation>, IRecurringReservationRepository
    {
        public RecurringReservationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddReservationAsync(RecurringReservation reservation)
        {
            Create(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddReservationsCollectionAsync(IEnumerable<RecurringReservation> reservations)
        {
            CreateRange(reservations);

            return await SaveChangedAsync();
        }

        public async Task<RecurringReservation> GetReservationAsync(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public IQueryable<RecurringReservation> GetReservationsCollection(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public async Task<IEnumerable<RecurringReservation>> GetReservationsCollectionAsync(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public bool ReservationExists(Expression<Func<RecurringReservation, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveReservationAsync(RecurringReservation reservation)
        {
            Delete(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateReservationAsync(RecurringReservation reservation)
        {
            Update(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateReservationsCollectionAsync(IEnumerable<RecurringReservation> reservations)
        {
            UpdateRange(reservations);

            return await SaveChangedAsync();
        }

        private async Task<RecurringReservation> GetReservationWithEagerLoadingAsync(Expression<Func<RecurringReservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .Include(r => r.Reservation)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private async Task<RecurringReservation> GetReservationWithoutEagerLoadingAsync(Expression<Func<RecurringReservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private IQueryable<RecurringReservation> GetReservationsCollectionWithEagerLoading(Expression<Func<RecurringReservation, bool>> expression)
        {
            var reservations = Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .Include(r => r.Reservation);

            return reservations;
        }
    }
}
