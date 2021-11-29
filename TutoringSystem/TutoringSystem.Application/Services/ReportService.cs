﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IAdditionalOrderRepository orderRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IStudentTutorRepository studentTutorRepository;
        private readonly ISortHelper<StudentReportDto> studentSortHelper;
        private readonly ISortHelper<SubjectReportDto> subjectSortHelper;
        private readonly ISortHelper<PlaceReportDto> placeSortHelper;
        private readonly ISortHelper<SubjectCategoryReportDto> subjectCategorySortHelper;

        public ReportService(IReservationRepository reservationRepository,
            IAdditionalOrderRepository orderRepository,
            ISubjectRepository subjectRepository,
            IStudentTutorRepository studentTutorRepository, 
            ISortHelper<StudentReportDto> studentSortHelper, 
            ISortHelper<SubjectReportDto> subjectSortHelper, 
            ISortHelper<PlaceReportDto> placeSortHelper, 
            ISortHelper<SubjectCategoryReportDto> subjectCategorySortHelper)
        {
            this.reservationRepository = reservationRepository;
            this.orderRepository = orderRepository;
            this.subjectRepository = subjectRepository;
            this.studentTutorRepository = studentTutorRepository;
            this.studentSortHelper = studentSortHelper;
            this.subjectSortHelper = subjectSortHelper;
            this.placeSortHelper = placeSortHelper;
            this.subjectCategorySortHelper = subjectCategorySortHelper;
        }

        public async Task<TutorReportDto> GetGeneralReportAsync(long tutorId, ReportParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsCollectionAsync(GetExpressionToTutorReservations(tutorId, parameters));
            var orders = orderRepository.GetAdditionalOrdersCollection(GetExpressionToOrders(tutorId, parameters));

            var result = new TutorReportDto
            {
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TutoringProfit = reservations.Sum(r => r.Cost),
                OrderProfit = orders.Sum(o => o.Cost),
            };
            result.TotalProfit = result.OrderProfit + result.TutoringProfit;

            return result;
        }

        public async Task<IEnumerable<StudentReportDto>> GetStudentsReportAsync(long tutorId, ReportParameters parameters)
        {
            var students = await GetStudents(tutorId);
            var studentsSummary = students.Select(s => GetStudentSummaryAsync(s, tutorId, parameters).Result);
            studentsSummary = studentSortHelper.ApplySort(studentsSummary.AsQueryable(), parameters.OrderBy).AsEnumerable();

            return studentsSummary;
        }

        public async Task<IEnumerable<SubjectReportDto>> GetSubjectsReportAsync(long tutorId, ReportParameters parameters)
        {
            var subjects = await subjectRepository.GetSubjectsCollectionAsync(s => s.TutorId.Equals(tutorId), null);
            var subjectsSummary = subjects.Select(s => GetSubjectReportAsync(s, parameters).Result);
            subjectsSummary = subjectsSummary.Where(p => p != null);
            subjectsSummary = subjectSortHelper.ApplySort(subjectsSummary.AsQueryable(), parameters.OrderBy).AsEnumerable();

            return subjectsSummary;
        }

        public IEnumerable<SubjectCategoryReportDto> GetSubjectCategoriesReport(long tutorId, ReportParameters parameters)
        {
            var categories = Enum.GetValues<SubjectCategory>();
            var subjectCategoriesSummary = categories.Select(c => GetSubjectCategorySummaryAsync(tutorId, c, parameters).Result);
            subjectCategoriesSummary = subjectCategoriesSummary.Where(p => p != null);
            subjectCategoriesSummary = subjectCategorySortHelper.ApplySort(subjectCategoriesSummary.AsQueryable(), parameters.OrderBy).AsEnumerable();

            return subjectCategoriesSummary;
        }

        public IEnumerable<PlaceReportDto> GetPlacesReport(long tutorId, ReportParameters parameters)
        {
            var places = Enum.GetValues<ReservationPlace>();
            var placesSummary = places.Select(p => GetPlaceSummaryAsync(tutorId, p, parameters).Result);
            placesSummary = placesSummary.Where(p => p != null);
            placesSummary = placeSortHelper.ApplySort(placesSummary.AsQueryable(), parameters.OrderBy).AsEnumerable();

            return placesSummary;
        }

        private async Task<StudentReportDto> GetStudentSummaryAsync(Student student, long tutorId, ReportParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsCollectionAsync(GetExpressionToStudentReservations(student.Id, tutorId, parameters));

            return new StudentReportDto
            {
                Username = student.Username,
                StudentName = $"{student.FirstName} {student.LastName}",
                ReservationsCount = reservations.ToList().Count,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        private async Task<SubjectReportDto> GetSubjectReportAsync(Subject subject, ReportParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsCollectionAsync(GetExpressionToSubjectReservations(subject.Id, parameters));
            if (!subject.IsActive && reservations.ToList().Count == 0)
                return null;

            return new SubjectReportDto
            {
                SubjectName = subject.Name,
                ReservationsCount = reservations.ToList().Count,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        private async Task<SubjectCategoryReportDto> GetSubjectCategorySummaryAsync(long tutorId, SubjectCategory category, ReportParameters parameters)
        {
            if (!(await subjectRepository.GetSubjectsCollectionAsync(r => r.TutorId.Equals(tutorId) && r.Category.Equals(category))).Any())
                return null;

            var reservations = await reservationRepository.GetReservationsCollectionAsync(GetExpressionToSubjectCategoryReservations(tutorId, category, parameters));

            return new SubjectCategoryReportDto
            {
                SubjectCategory = category,
                ReservationsCount = reservations.ToList().Count,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        private async Task<PlaceReportDto> GetPlaceSummaryAsync(long tutorId, ReservationPlace place, ReportParameters parameters)
        {
            if (!(await reservationRepository.GetReservationsCollectionAsync(r => r.TutorId.Equals(tutorId) && r.Place.Equals(place))).Any())
                return null;

            var reservations = await reservationRepository.GetReservationsCollectionAsync(GetExpressionToPlaceReservations(tutorId, place, parameters));

            return new PlaceReportDto
            {
                Place = place,
                ReservationsCount = reservations.ToList().Count,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        private async Task<IEnumerable<Student>> GetStudents(long tutorId)
        {
            var studentTutors = await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.TutorId.Equals(tutorId), null);

            return studentTutors is null ? new List<Student>() : studentTutors.Select(st => st.Student);
        }

        private Expression<Func<Reservation, bool>> GetExpressionToTutorReservations(long tutorId, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId) &&
                    r.StartTime.Date >= parameters.StartDate.Date &&
                    r.StartTime.Date <= parameters.EndDate.Date;

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToStudentReservations(long studentId, long tutorId, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.StudentId.Equals(studentId) &&
                    r.TutorId.Equals(tutorId) &&
                    r.StartTime.Date >= parameters.StartDate.Date &&
                    r.StartTime.Date <= parameters.EndDate.Date;

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToSubjectReservations(long subjectId, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.SubjectId.Equals(subjectId) &&
                    r.StartTime.Date >= parameters.StartDate.Date &&
                    r.StartTime.Date <= parameters.EndDate.Date;

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToSubjectCategoryReservations(long tutorId, SubjectCategory category, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId) &&
                    r.Subject.Category.Equals(category) &&
                    r.StartTime.Date >= parameters.StartDate.Date &&
                    r.StartTime.Date <= parameters.EndDate.Date;

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToPlaceReservations(long tutorId, ReservationPlace place, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId) &&
                    r.Place.Equals(place) &&
                    r.StartTime.Date >= parameters.StartDate.Date &&
                    r.StartTime.Date <= parameters.EndDate.Date;

            return expression;
        }

        private Expression<Func<AdditionalOrder, bool>> GetExpressionToOrders(long tutorId, ReportParameters parameters)
        {
            Expression<Func<AdditionalOrder, bool>> expression = r => r.TutorId.Equals(tutorId) &&
                    r.ReceiptDate.Date >= parameters.StartDate.Date &&
                    r.ReceiptDate.Date <= parameters.EndDate.Date;

            return expression;
        }
    }
}
