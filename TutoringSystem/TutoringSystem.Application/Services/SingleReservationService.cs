using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class SingleReservationService : ISingleReservationService
    {
        private readonly ISingleReservationRepository reservationRepository;
        private readonly IIntervalRepository intervalRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IAvailabilityRepository availabilityRepository;
        private readonly IMapper mapper;

        public SingleReservationService(ISingleReservationRepository reservationRepository,
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

        public async Task<ReservationDto> AddReservationByStudentAsync(long studentId, NewStudentSingleReservationDto newReservation)
        {
            if (!await ValidateNewStudentReservationAsync(newReservation))
            {
                return null;
            }

            var reservation = mapper.Map<SingleReservation>(newReservation);
            reservation.StudentId = studentId;
            reservation.Cost = await CalculateReservationCost(studentId, newReservation);

            bool created = false;
            if (await UpdateAvailabilityStudentAsync(newReservation))
            {
                created = await reservationRepository.AddReservationAsync(reservation);
            }

            return created ? mapper.Map<ReservationDto>(reservation) : null;
        }

        public async Task<ReservationDto> AddReservationByTutorAsync(long tutorId, NewTutorSingleReservationDto newReservation)
        {
            if (!await ValidateNewTutorReservationAsync(tutorId, newReservation))
            {
                return null;
            }

            var reservation = mapper.Map<SingleReservation>(newReservation);
            reservation.TutorId = tutorId;
            reservation.Cost = await CalculateReservationCost(tutorId, newReservation);

            bool created = false;
            if (await UpdateAvailabilityTutorAsync(tutorId, newReservation))
            {
                created = await reservationRepository.AddReservationAsync(reservation);
            }

            return created ? mapper.Map<ReservationDto>(reservation) : null;
        }

        public async Task<bool> RemoveReservationAsync(long reservationId)
        {
            var reservation = await reservationRepository.GetReservationAsync(r => r.Id.Equals(reservationId));

            return await reservationRepository.RemoveReservationAsync(reservation);
        }

        public async Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            var reservation = await reservationRepository.GetReservationAsync(r => r.Id.Equals(reservationId), isEagerLoadingEnabled: true);

            return mapper.Map<ReservationDetailsDto>(reservation);
        }

        public async Task<PagedList<ReservationDto>> GetReservationsByStudentAsync(long studentId, ReservationParameters parameters)
        {
            Expression<Func<SingleReservation, bool>> expression = r => r.StudentId.Equals(studentId);
            FilterByDate(ref expression, parameters);
            FilterByPlace(ref expression, parameters);
            var resevations = await reservationRepository.GetReservationsCollectionAsync(expression, isEagerLoadingEnabled: true);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters)
        {
            Expression<Func<SingleReservation, bool>> expression = r => r.TutorId.Equals(tutorId);
            FilterByDate(ref expression, parameters);
            FilterByPlace(ref expression, parameters);
            var resevations = await reservationRepository.GetReservationsCollectionAsync(expression, isEagerLoadingEnabled: true);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<bool> UpdateTutorReservationAsync(UpdatedTutorReservationDto updatedReservation)
        {
            if (!await ValidateUpdatedTutorReservationAsync(updatedReservation))
            {
                return false;
            }

            var existingReservation = await reservationRepository.GetReservationAsync(r => r.Id.Equals(updatedReservation.Id));
            var reservation = mapper.Map(updatedReservation, existingReservation);

            return await reservationRepository.UpdateReservationAsync(reservation);
        }

        public async Task<bool> UpdateStudentReservationAsync(UpdatedStudentReservationDto updatedReservation)
        {
            if (!await ValidateUpdatedStudentReservationAsync(updatedReservation))
            {
                return false;
            }

            var existingReservation = await reservationRepository.GetReservationAsync(r => r.Id.Equals(updatedReservation.Id));
            var reservation = mapper.Map(updatedReservation, existingReservation);

            return await reservationRepository.UpdateReservationAsync(reservation);
        }

        private async Task<double> CalculateReservationCost(long tutorId, NewTutorSingleReservationDto newReservation)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(newReservation.StudentId));
            double cost = newReservation.Cost ?? student.GetHourRate(tutorId) * (newReservation.Duration / 60.0);

            return cost;
        }

        private async Task<double> CalculateReservationCost(long studentId, NewStudentSingleReservationDto newReservation)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            double cost = student.GetHourRate(newReservation.TutorId) * (newReservation.Duration / 60.0);

            return cost;
        }

        private static void FilterByPlace(ref Expression<Func<SingleReservation, bool>> expression, ReservationParameters parameters)
        {
            if (parameters.IsAtTutor && parameters.IsAtStudent && parameters.IsOnline)
                return;

            if (parameters.IsAtTutor && !parameters.IsAtStudent && !parameters.IsOnline)
                ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(ReservationPlace.AtTutor));
            else if (!parameters.IsAtTutor && parameters.IsAtStudent && !parameters.IsOnline)
                ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(ReservationPlace.AtStudent));
            else if (!parameters.IsAtTutor && !parameters.IsAtStudent && parameters.IsOnline)
                ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(ReservationPlace.Online));
            else if (parameters.IsAtTutor && parameters.IsAtStudent && !parameters.IsOnline)
                ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(ReservationPlace.AtTutor) || r.Place.Equals(ReservationPlace.AtStudent));
            else if (parameters.IsAtTutor && !parameters.IsAtStudent && parameters.IsOnline)
                ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(ReservationPlace.AtTutor) || r.Place.Equals(ReservationPlace.Online));
            else if (!parameters.IsAtTutor && parameters.IsAtStudent && parameters.IsOnline)
                ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(ReservationPlace.AtStudent) || r.Place.Equals(ReservationPlace.Online));
        }

        private static void FilterByDate(ref Expression<Func<SingleReservation, bool>> expression, ReservationParameters parameters)
        {
            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime.Date >= parameters.StartDate.Date &&
                    r.StartTime.AddMinutes(r.Duration).Date <= parameters.EndDate.Date);
        }

        private async Task<bool> ValidateNewStudentReservationAsync(NewStudentSingleReservationDto reservation)
        {
            if (reservation.StartTime.AddMinutes(30) < DateTime.Now)
                return false;

            var interval = await intervalRepository.GetIntervalAsync(i => i.Id.Equals(reservation.IntervalId));
            if (interval is null)
                return false;
            else if (reservation.StartTime < interval.StartTime || reservation.StartTime.AddMinutes(reservation.Duration) > interval.EndTime)
                return false;

            return true;
        }

        private async Task<bool> ValidateNewTutorReservationAsync(long tutorId, NewTutorSingleReservationDto reservation)
        {
            var reservations = await reservationRepository.GetReservationsCollectionAsync(r => r.TutorId.Equals(tutorId) && r.StartTime.Date.Equals(reservation.StartTime.Date));
            if (reservation is null)
            {
                return true;
            }

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

        private async Task<bool> UpdateAvailabilityStudentAsync(NewStudentSingleReservationDto reservation)
        {
            var interval = await intervalRepository.GetIntervalAsync(i => i.Id.Equals(reservation.IntervalId));
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

        private async Task<bool> UpdateAvailabilityTutorAsync(long tutorId, NewTutorSingleReservationDto reservation)
        {
            var availability = await availabilityRepository.GetAvailabilityAsync(a => a.TutorId.Equals(tutorId) && a.Date.Date.Equals(reservation.StartTime.Date));
            if (availability is null)
                return true;

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