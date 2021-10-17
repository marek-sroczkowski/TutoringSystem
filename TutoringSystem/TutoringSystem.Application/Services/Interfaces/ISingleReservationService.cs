using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface ISingleReservationService
    {
        Task<PagedList<ReservationDto>> GetReservationsByStudentAsync(long studentId, ReservationParameters parameters);
        Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters);
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        Task<ReservationDto> AddReservationByStudentAsync(long studentId, NewStudentSingleReservationDto newReservation);
        Task<ReservationDto> AddReservationByTutorAsync(long tutorId, NewTutorSingleReservationDto newReservation);
        Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation);
        Task<bool> DeleteReservationAsync(long reservationId);
        Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation);
    }
}
