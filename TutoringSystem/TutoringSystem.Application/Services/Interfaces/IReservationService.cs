using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Reservation;
using TutoringSystem.Application.Models.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        PagedList<ReservationDto> GetReservationsByStudent(long studentId, ReservationParameters parameters);
        PagedList<ReservationDto> GetReservationsByTutor(long tutorId, ReservationParameters parameters);
    }
}
