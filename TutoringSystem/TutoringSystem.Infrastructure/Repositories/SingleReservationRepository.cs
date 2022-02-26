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

        public async Task<bool> AddReservationsCollectionAsync(IEnumerable<SingleReservation> reserservations)
        {
            CreateRange(reserservations);

            return await SaveChangedAsync();
        }

        public async Task<SingleReservation> GetReservationAsync(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public IQueryable<SingleReservation> GetReservationsCollection(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public bool IsReservationExist(Expression<Func<SingleReservation, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveReservationAsync(SingleReservation reservation)
        {
            reservation.IsActive = false;

            return await UpdateReservationAsync(reservation);
        }

        public async Task<bool> UpdateReservationAsync(SingleReservation reservation)
        {
            Update(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateReservationsCollectionAsync(IEnumerable<SingleReservation> reservations)
        {
            UpdateRange(reservations);

            return await SaveChangedAsync();
        }

        private async Task<SingleReservation> GetReservationWithEagerLoadingAsync(Expression<Func<SingleReservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private async Task<SingleReservation> GetReservationWithoutEagerLoadingAsync(Expression<Func<SingleReservation, bool>> expression)
        {
            var reservation = await Find(expression)
                .FirstOrDefaultAsync();

            return reservation;
        }

        private IQueryable<SingleReservation> GetReservationsCollectionWithEagerLoading(Expression<Func<SingleReservation, bool>> expression)
        {
            var reservations = Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor);

            return reservations;
        }
    }
}