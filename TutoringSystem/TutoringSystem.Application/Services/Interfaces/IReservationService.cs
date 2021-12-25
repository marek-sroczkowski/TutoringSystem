using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId);
        PagedList<ReservationDto> GetReservationsByStudent(long studentId, ReservationParameters parameters);
        PagedList<ReservationDto> GetReservationsByTutor(long tutorId, ReservationParameters parameters);
    }
}
