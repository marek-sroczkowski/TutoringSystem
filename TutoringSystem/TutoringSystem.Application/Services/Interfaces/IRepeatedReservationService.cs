using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IRepeatedReservationService
    {
        Task<IEnumerable<RepeatedReservationDto>> GetReservationsByStudent(long studentId);
        Task<IEnumerable<RepeatedReservationDto>> GetReservationsByTutor(long tutorId);
    }
}