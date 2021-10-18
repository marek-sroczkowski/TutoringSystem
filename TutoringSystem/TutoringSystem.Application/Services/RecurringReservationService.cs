using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class RecurringReservationService : IRecurringReservationService
    {
        private readonly IRecurringReservationRepository reservationRepository;
        private readonly IRepeatedReservationRepository repeatedReservationRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public RecurringReservationService(IRecurringReservationRepository reservationRepository,
            IRepeatedReservationRepository repeatedReservationRepository,
            IStudentRepository studentRepository,
            IMapper mapper)
        {
            this.reservationRepository = reservationRepository;
            this.repeatedReservationRepository = repeatedReservationRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<ReservationDto> AddReservationByStudentAsync(long studentId, 
            NewStudentRecurringReservationDto newReservation)
        {
            var reservation = mapper.Map<RecurringReservation>(newReservation);
            reservation.StudentId = studentId;
            reservation.Cost = await CalculateReservationCost(studentId, newReservation);
            await repeatedReservationRepository.AddReservationAsync(new RepeatedReservation(reservation));

            return mapper.Map<ReservationDto>(reservation);
        }

        public async Task<ReservationDto> AddReservationByTutorAsync(long tutorId, 
            NewTutorRecurringReservationDto newReservation)
        {
            var reservation = mapper.Map<RecurringReservation>(newReservation);
            reservation.TutorId = tutorId;
            reservation.Cost = await CalculateReservationCost(newReservation);
            await repeatedReservationRepository.AddReservationAsync(new RepeatedReservation(reservation));

            return mapper.Map<ReservationDto>(reservation);
        }

        public async Task<bool> DeleteReservationAsync(long reservationId)
        {
            var reservation = await reservationRepository.GetReservationAsync(r => r.Id.Equals(reservationId));
            var deleted = await reservationRepository.DeleteReservationAsync(reservation) && 
                await repeatedReservationRepository.DeleteReservationAsync(reservation.Reservation);

            return deleted;
        }

        public async Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            var reservation = await reservationRepository.GetReservationAsync(r => r.Id.Equals(reservationId));

            return mapper.Map<ReservationDetailsDto>(reservation);
        }

        public async Task<PagedList<ReservationDto>> GetReservationsByStudentAsync(long studentId, ReservationParameters parameters)
        {
            Expression<Func<RecurringReservation, bool>> expression = r => r.StudentId.Equals(studentId);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);
            FilterByPlace(ref expression, parameters.Place);
            var resevations = await reservationRepository.GetReservationsCollectionAsync(expression);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters)
        {
            Expression<Func<RecurringReservation, bool>> expression = r => r.TutorId.Equals(tutorId);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);
            FilterByPlace(ref expression, parameters.Place);
            var resevations = await reservationRepository.GetReservationsCollectionAsync(expression);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        private void FilterByStartDate(ref Expression<Func<RecurringReservation, bool>> expression, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime >= startDate.Value);
        }

        private void FilterByEndDate(ref Expression<Func<RecurringReservation, bool>> expression, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime <= endDate.Value);
        }

        private void FilterByPlace(ref Expression<Func<RecurringReservation, bool>> expression, ReservationPlace? place)
        {
            if (!place.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(place.Value));
        }

        private async Task<double> CalculateReservationCost(NewTutorRecurringReservationDto newReservation)
        {
            double cost = newReservation.Cost.HasValue ?
                newReservation.Cost.Value :
                (await studentRepository.GetStudentAsync(s => s.Id.Equals(newReservation.StudentId))).HourlRate * (newReservation.Duration / 60.0);

            return cost;
        }

        private async Task<double> CalculateReservationCost(long studentId, NewStudentRecurringReservationDto newReservation)
        {
            double cost = (await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId))).HourlRate * (newReservation.Duration / 60.0);

            return cost;
        }
    }
}
