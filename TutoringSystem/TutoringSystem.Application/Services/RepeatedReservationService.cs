using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class RepeatedReservationService : IRepeatedReservationService
    {
        private readonly IRepeatedReservationRepository reservationRepository;
        private readonly IMapper mapper;

        public RepeatedReservationService(IRepeatedReservationRepository reservationRepository,
            IMapper mapper)
        {
            this.reservationRepository = reservationRepository;
            this.mapper = mapper;
        }

        public IEnumerable<RepeatedReservationDto> GetReservationsByStudent(long studentId)
        {
            var resevations = reservationRepository.GetReservationsCollection(r => r.StudentId.Equals(studentId));

            return mapper.Map<IEnumerable<RepeatedReservationDto>>(resevations);
        }

        public IEnumerable<RepeatedReservationDto> GetReservationsByTutor(long tutorId)
        {
            var resevations = reservationRepository.GetReservationsCollection(r => r.TutorId.Equals(tutorId));

            return mapper.Map<IEnumerable<RepeatedReservationDto>>(resevations);
        }
    }
}