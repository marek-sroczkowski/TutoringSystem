using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Reservation;
using TutoringSystem.Application.Models.Enums;
using TutoringSystem.Application.Models.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IRecurringReservationService
    {
        Task<ReservationDto> AddReservationByStudentAsync(long studentId, NewStudentRecurringReservationDto newReservation);
        Task<ReservationDto> AddReservationByTutorAsync(long tutorId, NewTutorRecurringReservationDto newReservation);
        Task<bool> RemoveReservationAsync(long reservationId, RecurringReservationRemovingMode mode);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<PagedList<ReservationDto>> GetReservationsByStudentAsync(long studentId, ReservationParameters parameters);
        Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters);
    }
}
