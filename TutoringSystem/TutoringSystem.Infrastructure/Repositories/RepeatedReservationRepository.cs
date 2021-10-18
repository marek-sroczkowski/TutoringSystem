using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
            Delete(reservation);

            return await SaveChangedAsync();
        }

        public async Task<RepeatedReservation> GetReservationAsync(Expression<Func<RepeatedReservation, bool>> expression)
        {
            var reservation = await DbContext.RepeatedReservations
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(expression);

            return reservation;
        }

        public async Task<IEnumerable<RepeatedReservation>> GetReservationsCollectionAsync(Expression<Func<RepeatedReservation, bool>> expression)
        {
            var reservations = await FindByCondition(expression)
                .Include(r => r.Reservations)
                .ToListAsync();

            return reservations;
        }

        public async Task<bool> UpdateReservationAsync(RepeatedReservation updatedReservation)
        {
            Update(updatedReservation);

            return await SaveChangedAsync();
        }
    }
}
