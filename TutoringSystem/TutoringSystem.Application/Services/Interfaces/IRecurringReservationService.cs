using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IRecurringReservationService
    {
        Task<ReservationDto> AddReservationByStudentAsync(long studentId, NewStudentRecurringReservationDto newReservation);
        Task<ReservationDto> AddReservationByTutorAsync(long tutorId, NewTutorRecurringReservationDto newReservation);
        Task<bool> DeleteReservationAsync(long reservationId);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<PagedList<ReservationDto>> GetReservationsByStudentAsync(long studentId, ReservationParameters parameters);
        Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters);
    }
}
