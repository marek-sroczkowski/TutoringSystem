using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IReservationRepository
    {
        Task<bool> AddReservationAsync(Reservation reservation);
        Task<Reservation> GetReservationByIdAsync(long reservationId);
        Task<IEnumerable<Reservation>> GetReservationsByStudentIdAsync(long studentId);
        Task<IEnumerable<Reservation>> GetReservationsByTutorIdAsync(long tutorId);
        Task<bool> UpdateReservationAsync(Reservation updatedReservation);
        Task<bool> DeleteReservationAsync(Reservation reservation);
        Task<IEnumerable<Reservation>> GetReservationsByTutorIdAndDateAsync(long tutorId, DateTime date);
    }
}
