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

        public async Task<bool> DeleteReservationAsync(RecurringReservation reservation)
        {
            Delete(reservation);

            return await SaveChangedAsync();
        }

        public async Task<RecurringReservation> GetReservationAsync(Expression<Func<RecurringReservation, bool>> expression)
        {
            var reservation = await DbContext.RecurringReservations
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(expression);

            return reservation;
        }

        public async Task<IEnumerable<RecurringReservation>> GetReservationsCollectionAsync(Expression<Func<RecurringReservation, bool>> expression)
        {
            var reservations = await FindByCondition(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .ToListAsync();

            return reservations;
        }

        public async Task<bool> UpdateReservationAsync(RecurringReservation updatedReservation)
        {
            Update(updatedReservation);

            return await SaveChangedAsync();
        }
    }
}
