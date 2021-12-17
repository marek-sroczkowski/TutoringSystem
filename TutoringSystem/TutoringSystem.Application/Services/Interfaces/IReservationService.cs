using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IReservationService
    {
        PagedList<ReservationDto> GetReservationsByStudent(long studentId, ReservationParameters parameters);
        PagedList<ReservationDto> GetReservationsByTutor(long tutorId, ReservationParameters parameters);
    }
}
