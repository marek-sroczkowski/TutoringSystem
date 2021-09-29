using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Helpers;
using TutoringSystem.Domain.Parameters;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IMapper mapper;

        public ReservationService(IReservationRepository reservationRepository, IMapper mapper)
        {
            this.reservationRepository = reservationRepository;
            this.mapper = mapper;
        }

        public async Task<ReservationDto> AddReservationByStudentAsync(long studentId, NewStudentReservationDto newReservation)
        {
            var reservation = mapper.Map<Reservation>(newReservation);
            reservation.StudentId = studentId;
            var created = await reservationRepository.AddReservationAsync(reservation);

            if (!created)
                return null;

            return mapper.Map<ReservationDto>(reservation);
        }

        public async Task<ReservationDto> AddReservationByTutorAsync(long tutorId, NewTutorReservationDto newReservation)
        {
            var reservation = mapper.Map<Reservation>(newReservation);
            reservation.TutorId = tutorId;
            var created = await reservationRepository.AddReservationAsync(reservation);

            if (!created)
                return null;

            return mapper.Map<ReservationDto>(reservation);
        }

        public async Task<bool> DeleteReservationAsync(long reservationId)
        {
            var reservation = await reservationRepository.GetReservationByIdAsync(reservationId);

            return await reservationRepository.DeleteReservationAsync(reservation);
        }

        public async Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            var reservation = await reservationRepository.GetReservationByIdAsync(reservationId);

            return mapper.Map<ReservationDetailsDto>(reservation);
        }

        public async Task<PagedList<ReservationDto>> GetReservationsByStudentAsync(long studentId, ReservationParameters parameters)
        {
            var resevations = await reservationRepository.GetReservationsByStudentIdAsync(studentId);

            FilterByStartDate(ref resevations, parameters.StartDate);
            FilterByEndDate(ref resevations, parameters.EndDate);
            FilterByPlace(ref resevations, parameters.Place);

            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters)
        {
            var resevations = await reservationRepository.GetReservationsByTutorIdAsync(tutorId);

            FilterByStartDate(ref resevations, parameters.StartDate);
            FilterByEndDate(ref resevations, parameters.EndDate);
            FilterByPlace(ref resevations, parameters.Place);

            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> UpdateReservationAsync(UpdatedReservationDto updatedReservation)
        {
            var existingReservation = await reservationRepository.GetReservationByIdAsync(updatedReservation.Id);
            var reservation = mapper.Map(updatedReservation, existingReservation);

            return await reservationRepository.UpdateReservationAsync(reservation);
        }

        private void FilterByStartDate(ref IEnumerable<Reservation> reservations, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            reservations = reservations.Where(r => r.StartTime >= startDate.Value);
        }

        private void FilterByEndDate(ref IEnumerable<Reservation> reservations, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            reservations = reservations.Where(r => r.StartTime <= endDate.Value);
        }

        private void FilterByPlace(ref IEnumerable<Reservation> reservations, ReservationPlace? place)
        {
            if (!place.HasValue)
                return;

            reservations = reservations.Where(r => r.Place.Equals(place.Value));
        }
    }
}
