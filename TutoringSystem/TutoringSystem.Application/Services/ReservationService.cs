using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Reservation;
using TutoringSystem.Application.Models.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IMapper mapper;
        private readonly ISortHelper<Reservation> sortHelper;

        public ReservationService(IReservationRepository reservationRepository,
            IMapper mapper,
            ISortHelper<Reservation> sortHelper)
        {
            this.reservationRepository = reservationRepository;
            this.mapper = mapper;
            this.sortHelper = sortHelper;
        }

        public PagedList<ReservationDto> GetReservationsByStudent(long studentId, ReservationParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.StudentId.Equals(studentId);
            FilterByDate(ref expression, parameters);
            FilterByPlace(ref expression, parameters);
            var resevations = sortHelper.ApplySort(reservationRepository.GetReservationsCollection(expression, isEagerLoadingEnabled: true), parameters.OrderBy);
            var reservationDtos = mapper.Map<IEnumerable<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public PagedList<ReservationDto> GetReservationsByTutor(long tutorId, ReservationParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId);
            FilterByDate(ref expression, parameters);
            FilterByPlace(ref expression, parameters);
            var resevations = sortHelper.ApplySort(reservationRepository.GetReservationsCollection(expression, isEagerLoadingEnabled: true), parameters.OrderBy);
            var reservationDtos = mapper.Map<IEnumerable<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<ReservationDetailsDto> GetReservationByIdAsync(long reservationId)
        {
            var reservation = await reservationRepository.GetReservationAsync(r => r.Id.Equals(reservationId), isEagerLoadingEnabled: true);

            return mapper.Map<ReservationDetailsDto>(reservation);
        }

        private static void FilterByPlace(ref Expression<Func<Reservation, bool>> expression, ReservationParameters parameters)
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

        private static void FilterByDate(ref Expression<Func<Reservation, bool>> expression, ReservationParameters parameters)
        {
            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime.Date >= parameters.StartDate.Date &&
            r.StartTime.AddMinutes(r.Duration).Date <= parameters.EndDate.Date);
        }
    }
}