using AutoMapper;
using System;
using System.Collections.Generic;
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
            reservation.Cost = await CalculateReservationCost(tutorId, newReservation);
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
            FilterByDate(ref expression, parameters);
            FilterByPlace(ref expression, parameters);
            var resevations = await reservationRepository.GetReservationsCollectionAsync(expression);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<ReservationDto>> GetReservationsByTutorAsync(long tutorId, ReservationParameters parameters)
        {
            Expression<Func<RecurringReservation, bool>> expression = r => r.TutorId.Equals(tutorId);
            FilterByDate(ref expression, parameters);
            FilterByPlace(ref expression, parameters);
            var resevations = await reservationRepository.GetReservationsCollectionAsync(expression);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        private void FilterByPlace(ref Expression<Func<RecurringReservation, bool>> expression, ReservationParameters parameters)
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

        private void FilterByDate(ref Expression<Func<RecurringReservation, bool>> expression, ReservationParameters parameters)
        {
            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime.Date >= parameters.StartDate.Date &&
            r.StartTime.AddMinutes(r.Duration).Date <= parameters.EndDate.Date);
        }

        private async Task<double> CalculateReservationCost(long tutorId, NewTutorRecurringReservationDto newReservation)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(newReservation.StudentId));
            double cost = newReservation.Cost.HasValue ?
                newReservation.Cost.Value :
                student.GetHourRate(tutorId) * (newReservation.Duration / 60.0);

            return cost;
        }

        private async Task<double> CalculateReservationCost(long studentId, NewStudentRecurringReservationDto newReservation)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            double cost = student.GetHourRate(newReservation.TutorId) * (newReservation.Duration / 60.0);

            return cost;
        }
    }
}
