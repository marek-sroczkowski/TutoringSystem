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
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {
        public ReservationRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddReservationAsync(Reservation reservation)
        {
            Create(reservation);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteReservationAsync(Reservation reservation)
        {
            Delete(reservation);

            return await SaveChangedAsync();
        }

        public async Task<Reservation> GetReservationAsync(Expression<Func<Reservation, bool>> expression)
        {
            var reservation = await DbContext.Reservations
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(expression);

            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsCollectionAsync(Expression<Func<Reservation, bool>> expression)
        {
            var reservations = await FindByCondition(expression)
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .ToListAsync();

            return reservations;
        }

        public async Task<bool> UpdateReservationAsync(Reservation updatedReservation)
        {
            Update(updatedReservation);

            return await SaveChangedAsync();
        }
    }
}
