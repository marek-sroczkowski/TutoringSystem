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
        private readonly IIntervalRepository intervalRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IMapper mapper;

        public ReservationService(IReservationRepository reservationRepository,
            IIntervalRepository intervalRepository,
            IStudentRepository studentRepository,
            IAvailabilityRepository availabilityRepository,
            IMapper mapper)
        {
            this.reservationRepository = reservationRepository;
            this.intervalRepository = intervalRepository;
            this.studentRepository = studentRepository;
            this.availabilityRepository = availabilityRepository;
            this.mapper = mapper;
        }

        public async Task<ReservationDto> AddReservationByStudentAsync(long studentId, NewStudentReservationDto newReservation)
        {
            if (!(await ValidateNewStudentReservationAsync(newReservation)))
                return null;

            var reservation = mapper.Map<Reservation>(newReservation);
            reservation.StudentId = studentId;
            reservation.Cost = (await studentRepository.GetStudentByIdAsync(studentId)).HourlRate * (newReservation.Duration / 60.0);
            bool created = false;

            if (await UpdateAvailabilityStudentAsync(newReservation))
                created = await reservationRepository.AddReservationAsync(reservation);

            if (!created)
                return null;

            return mapper.Map<ReservationDto>(reservation);
        }

        public async Task<ReservationDto> AddReservationByTutorAsync(long tutorId, NewTutorReservationDto newReservation)
        {
            if (!(await ValidateNewTutorReservationAsync(tutorId, newReservation)))
                return null;

            var reservation = mapper.Map<Reservation>(newReservation);
            reservation.TutorId = tutorId;
            reservation.Cost = newReservation.Cost.HasValue ?
                newReservation.Cost.Value :
                (await studentRepository.GetStudentByIdAsync(newReservation.StudentId)).HourlRate * (newReservation.Duration / 60.0);
            bool created = false;
            if (await UpdateAvailabilityTutorAsync(tutorId, newReservation))
                created = await reservationRepository.AddReservationAsync(reservation);

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

        public async Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation)
        {
            if (!(await ValidateUpdatedTutorReservationAsync(updatedReservation)))
                return false;

            var existingReservation = await reservationRepository.GetReservationByIdAsync(updatedReservation.Id);
            var reservation = mapper.Map(updatedReservation, existingReservation);

            return await reservationRepository.UpdateReservationAsync(reservation);
        }

        public async Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation)
        {
            if (!(await ValidateUpdatedStudentReservationAsync(updatedReservation)))
                return false;

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

        private async Task<bool> ValidateNewStudentReservationAsync(NewStudentReservationDto reservation)
        {
            if (reservation.StartTime.AddMinutes(30) < DateTime.Now)
                return false;

            var interval = await intervalRepository.GetIntervalByIdAsync(reservation.IntervalId);
            if (interval is null)
                return false;
            else if (reservation.StartTime < interval.StartTime || reservation.StartTime.AddMinutes(reservation.Duration) > interval.EndTime)
                return false;

            return true;
        }

        private async Task<bool> ValidateNewTutorReservationAsync(long tutorId, NewTutorReservationDto reservation)
        {
            var reservations = await reservationRepository.GetReservationsByTutorIdAndDateAsync(tutorId, reservation.StartTime);
            if (reservation is null)
                return true;

            foreach (var r in reservations)
            {
                if ((reservation.StartTime >= r.StartTime && reservation.StartTime.AddMinutes(reservation.Duration) <= r.StartTime.AddMinutes(r.Duration)) ||
                    (reservation.StartTime <= r.StartTime && reservation.StartTime.AddMinutes(reservation.Duration) >= r.StartTime.AddMinutes(r.Duration)) ||
                    (reservation.StartTime < r.StartTime && reservation.StartTime.AddMinutes(reservation.Duration) < r.StartTime.AddMinutes(r.Duration) && reservation.StartTime.AddMinutes(reservation.Duration) > r.StartTime) ||
                    (reservation.StartTime > r.StartTime && reservation.StartTime.AddMinutes(reservation.Duration) > r.StartTime.AddMinutes(r.Duration) && reservation.StartTime < r.StartTime.AddMinutes(r.Duration)))
                    return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdatedStudentReservationAsync(UpdatedStudentReservationDto reservation)
        {
            return true;
        }

        private async Task<bool> ValidateUpdatedTutorReservationAsync(UpdatedTutorReservationDto reservation)
        {
            return true;
        }

        private async Task<bool> UpdateAvailabilityStudentAsync(NewStudentReservationDto reservation)
        {
            var interval = await intervalRepository.GetIntervalByIdAsync(reservation.IntervalId);
            var availability = interval.Availability;

            if (interval.StartTime == reservation.StartTime)
            {
                interval.StartTime = interval.StartTime.AddMinutes(reservation.Duration + availability.BreakTime);
            }
            else if (interval.EndTime == reservation.StartTime.AddMinutes(reservation.Duration))
            {
                interval.EndTime = interval.EndTime.AddMinutes((reservation.Duration + availability.BreakTime) * (-1));
            }
            else
            {
                var newInterval1 = new Interval { StartTime = interval.StartTime, EndTime = reservation.StartTime.AddMinutes(availability.BreakTime * (-1)) };
                var newInterval2 = new Interval { StartTime = reservation.StartTime.AddMinutes(reservation.Duration + availability.BreakTime), EndTime = interval.EndTime };

                if (newInterval1.StartTime.AddMinutes(30) <= newInterval1.EndTime)
                    availability.Intervals.Add(newInterval1);
                if (newInterval2.StartTime.AddMinutes(30) <= newInterval2.EndTime)
                    availability.Intervals.Add(newInterval2);

                availability.Intervals.Remove(interval);
            }

            if (interval.StartTime.AddMinutes(30) > interval.EndTime)
                availability.Intervals.Remove(interval);

            return await availabilityRepository.UpdateAvailabilityAsync(availability);
        }

        private async Task<bool> UpdateAvailabilityTutorAsync(long tutorId, NewTutorReservationDto reservation)
        {
            var availability = await availabilityRepository.GetAvailabilityByTutorIdAndDateAsync(tutorId, reservation.StartTime);

            for (int i = 0; i<availability.Intervals.Count; i++)
            {
                var interval = availability.Intervals.ElementAt(i);
                if (interval.StartTime == reservation.StartTime)
                {
                    interval.StartTime = interval.StartTime.AddMinutes(reservation.Duration + availability.BreakTime);
                }
                else if (interval.EndTime == reservation.StartTime.AddMinutes(reservation.Duration))
                {
                    interval.EndTime = interval.EndTime.AddMinutes((reservation.Duration + availability.BreakTime) * (-1));
                }
                else
                {
                    var newInterval1 = new Interval { StartTime = interval.StartTime, EndTime = reservation.StartTime.AddMinutes(availability.BreakTime * (-1)) };
                    var newInterval2 = new Interval { StartTime = reservation.StartTime.AddMinutes(reservation.Duration + availability.BreakTime), EndTime = interval.EndTime };

                    if (newInterval1.StartTime.AddMinutes(30) <= newInterval1.EndTime)
                        availability.Intervals.Add(newInterval1);
                    if (newInterval2.StartTime.AddMinutes(30) <= newInterval2.EndTime)
                        availability.Intervals.Add(newInterval2);

                    availability.Intervals.Remove(interval);
                }

                if (interval.StartTime.AddMinutes(30) > interval.EndTime)
                    availability.Intervals.Remove(interval);
            }

            return await availabilityRepository.UpdateAvailabilityAsync(availability);
        }
    }
}
