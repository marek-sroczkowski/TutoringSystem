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

        public async Task<bool> AddReservationsCollectionAsync(IEnumerable<RepeatedReservation> reservations)
        {
            CreateRange(reservations);

            return await SaveChangedAsync();
        }

        public async Task<RepeatedReservation> GetReservationAsync(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var reservation = isEagerLoadingEnabled
                ? await GetReservationWithEagerLoadingAsync(expression)
                : await GetReservationWithoutEagerLoadingAsync(expression);

            return reservation;
        }

        public IQueryable<RepeatedReservation> GetReservationsCollection(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var reservations = isEagerLoadingEnabled
                ? GetReservationsCollectionWithEagerLoading(expression)
                : Find(expression);

            return reservations;
        }

        public bool IsReservationExist(Expression<Func<RepeatedReservation, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveReservationAsync(RepeatedReservation reservation)
        {
            reservation.IsActive = false;

            return await UpdateReservationAsync(reservation);
        }

        public async Task<bool> UpdateReservationAsync(RepeatedReservation reservation)
        {
            Update(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateReservationsCollectionAsync(IEnumerable<RepeatedReservation> reservations)
        {
            UpdateRange(reservations);

            return await SaveChangedAsync();
        }

        private async Task<RepeatedReservation> GetReservationWithEagerLoadingAsync(Expression<Func<RepeatedReservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private async Task<RepeatedReservation> GetReservationWithoutEagerLoadingAsync(Expression<Func<RepeatedReservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private IQueryable<RepeatedReservation> GetReservationsCollectionWithEagerLoading(Expression<Func<RepeatedReservation, bool>> expression)
        {
            var reservations = Find(expression)
                .Include(r => r.Reservations);

            return reservations;
        }
    }
}