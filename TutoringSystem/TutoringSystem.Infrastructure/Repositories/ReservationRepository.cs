using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<Reservation> GetReservationByIdAsync(long reservationId)
        {
            var reservation = await DbContext.Reservations
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(r => r.Id.Equals(reservationId));

            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByStudentIdAsync(long studentId)
        {
            var reservations = FindByCondition(r => r.StudentId.Equals(studentId))
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor);

            return await reservations.ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByTutorIdAsync(long tutorId)
        {
            var reservations = FindByCondition(r => r.TutorId.Equals(tutorId))
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor);

            return await reservations.ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByTutorIdAndDateAsync(long tutorId, DateTime date)
        {
            var reservations = FindByCondition(r => r.TutorId.Equals(tutorId) && r.StartTime.Date.Equals(date.Date))
                .Include(r => r.Student)
                .Include(r => r.Subject)
                .Include(r => r.Tutor);

            return await reservations.ToListAsync();
        }

        public async Task<bool> UpdateReservationAsync(Reservation updatedReservation)
        {
            Update(updatedReservation);

            return await SaveChangedAsync();
        }
    }
}
