using System;
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

        public TutorReportDto GetGeneralReport(long tutorId, ReportParameters parameters)
        {
            var reservations = reservationRepository.GetReservationsCollection(GetExpressionToTutorReservations(tutorId, parameters));
            var orders = orderRepository.GetOrdersCollection(GetExpressionToOrders(tutorId, parameters));

            var result = new TutorReportDto
            {
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TutoringProfit = reservations.Sum(r => r.Cost),
                OrderProfit = orders.Sum(o => o.Cost),
            };
            result.TotalProfit = result.OrderProfit + result.TutoringProfit;

            return result;
        }

        public IEnumerable<GeneralTimedReportDto> GetGeneralTimedReport(long tutorId, ReportParameters parameters)
        {
            var intervals = new List<KeyValuePair<DateTime, DateTime>>();
            for(var date = parameters.StartDate; date <= parameters.EndDate; date = date.AddMonths(1))
            {
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                intervals.Add(KeyValuePair.Create(firstDayOfMonth, lastDayOfMonth));
            }

            var result = new List<GeneralTimedReportDto>();
            foreach (var inteval in intervals)
            {
                var report = GetGeneralReport(tutorId, new ReportParameters(inteval.Key, inteval.Value));
                result.Add(new GeneralTimedReportDto(inteval.Key, inteval.Value, report));
            }

            return result;
        }

        public async Task<IEnumerable<StudentReportDto>> GetStudentsReportAsync(long tutorId, ReportParameters parameters)
        {
            var students = await GetStudents(tutorId);
            var studentsSummary = students.Select(s => GetStudentSummary(s, tutorId, parameters));
            studentsSummary = studentSortHelper.ApplySort(studentsSummary.AsQueryable(), parameters.OrderBy).AsEnumerable();

            return studentsSummary;
        }

        public async Task<IEnumerable<SubjectReportDto>> GetSubjectsReportAsync(long tutorId, ReportParameters parameters)
        {
            var subjects = await subjectRepository.GetSubjectsCollectionAsync(s => s.TutorId.Equals(tutorId), null);
            var subjectsSummary = subjects.Select(s => GetSubjectReport(s, parameters));
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
            var placesSummary = places.Select(p => GetPlaceSummary(tutorId, p, parameters));
            placesSummary = placesSummary.Where(p => p != null);
            placesSummary = placeSortHelper.ApplySort(placesSummary.AsQueryable(), parameters.OrderBy).AsEnumerable();

            return placesSummary;
        }

        private StudentReportDto GetStudentSummary(Student student, long tutorId, ReportParameters parameters)
        {
            var reservations = reservationRepository.GetReservationsCollection(GetExpressionToStudentReservations(student.Id, tutorId, parameters));

            return new StudentReportDto
            {
                Username = student.Username,
                StudentName = $"{student.FirstName} {student.LastName}",
                ReservationsCount = reservations.Count(),
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        private SubjectReportDto GetSubjectReport(Subject subject, ReportParameters parameters)
        {
            var reservations = reservationRepository.GetReservationsCollection(GetExpressionToSubjectReservations(subject.Id, parameters));
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
            if (!subjectRepository.IsSubjectExist(r => r.TutorId.Equals(tutorId) && r.Category.Equals(category)))
            {
                return null;
            }

            var reservations = await reservationRepository.GetReservationsCollectionAsync(GetExpressionToSubjectCategoryReservations(tutorId, category, parameters));

            return new SubjectCategoryReportDto
            {
                SubjectCategory = category,
                ReservationsCount = reservations.Count(),
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        private PlaceReportDto GetPlaceSummary(long tutorId, ReservationPlace place, ReportParameters parameters)
        {
            if (!reservationRepository.IsReservationExist(r => r.TutorId.Equals(tutorId) && r.Place.Equals(place)))
            {
                return null;
            }

            var reservations = reservationRepository.GetReservationsCollection(GetExpressionToPlaceReservations(tutorId, place, parameters));

            return new PlaceReportDto
            {
                Place = place,
                ReservationsCount = reservations.Count(),
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