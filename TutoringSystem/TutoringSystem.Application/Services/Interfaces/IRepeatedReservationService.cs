using System.Collections.Generic;
using TutoringSystem.Application.Dtos.ReservationDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IRepeatedReservationService
    {
        IEnumerable<RepeatedReservationDto> GetReservationsByStudent(long studentId);
        IEnumerable<RepeatedReservationDto> GetReservationsByTutor(long tutorId);
    }
}