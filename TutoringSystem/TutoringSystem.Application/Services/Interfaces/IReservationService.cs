using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Domain.Helpers;
using TutoringSystem.Domain.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task<PagedList<ReservationDto>> GetReservationsByStudentAsync(long studentId, ReservationParameters parameters);
        Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<ReservationDto> AddReservationByStudentAsync(long studentId, CreateStudentReservationDto newReservation);
        Task<ReservationDto> AddReservationByTutorAsync(long tutorId, CreateTutorReservationDto newReservation);
        Task<bool> UpdateReservationAsync(long reservationId, UpdateReservationDto updatedReservation);
        Task<bool> DeleteReservationAsync(long reservationId);
    }
}
