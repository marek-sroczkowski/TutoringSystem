using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
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
            FilterByPlace(ref expression, parameters.Place);
            var resevations = sortHelper.ApplySort(reservationRepository.GetReservationsCollection(expression), parameters.OrderBy);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        public PagedList<ReservationDto> GetReservationsByTutor(long tutorId, ReservationParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId);
            FilterByDate(ref expression, parameters);
            FilterByPlace(ref expression, parameters.Place);
            var resevations = sortHelper.ApplySort(reservationRepository.GetReservationsCollection(expression), parameters.OrderBy);
            var reservationDtos = mapper.Map<ICollection<ReservationDto>>(resevations);

            return PagedList<ReservationDto>.ToPagedList(reservationDtos, parameters.PageNumber, parameters.PageSize);
        }

        private void FilterByPlace(ref Expression<Func<Reservation, bool>> expression, ReservationPlace? place)
        {
            if (!place.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.Place.Equals(place.Value));
        }

        private void FilterByDate(ref Expression<Func<Reservation, bool>> expression, ReservationParameters parameters)
        {
            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime.Date >= parameters.StartDate.Date &&
            r.StartTime.AddMinutes(r.Duration).Date <= parameters.EndDate.Date);
        }
    }
}