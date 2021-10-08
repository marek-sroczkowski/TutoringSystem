using AutoMapper;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IAdditionalOrderRepository orderRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;

        public ReportService(IReservationRepository reservationRepository, IAdditionalOrderRepository orderRepository, ITutorRepository tutorRepository, IStudentRepository studentRepository, ISubjectRepository subjectRepository, IMapper mapper)
        {
            this.reservationRepository = reservationRepository;
            this.orderRepository = orderRepository;
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }

        public async Task<TutorReportDto> GetReportByTutorAsync(long tutorId, ReportParameters parameters)
        {
            var tutor = await tutorRepository.GetTutorByIdAsync(tutorId);
            var students = await studentRepository.GetStudentsAsync(s => s.Tutors.Contains(tutor), null);
            var reservations = await reservationRepository.GetReservationsAsync(GetExpressionToTutorReservations(tutorId, parameters));
            var orders = await orderRepository.GetAdditionalOrdersAsync(GetExpressionToOrders(tutorId, parameters));

            var result = new TutorReportDto
            {
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TutoringProfit = reservations.Sum(r => r.Cost),
                OrderProfit = orders.Sum(o => o.Cost),
                StudentSummary = students.Select(s => GetStudentSummaryAsync(s.Id, parameters).Result)
            };
            result.TotalProfit = result.OrderProfit + result.TutoringProfit;

            return result;
        }

        public async Task<StudentSummaryDto> GetStudentSummaryAsync(long studentId, ReportParameters parameters)
        {
            var student = await studentRepository.GetStudentByIdAsync(studentId);
            var reservations = await reservationRepository.GetReservationsAsync(GetExpressionToStudentReservations(studentId, parameters));

            return new StudentSummaryDto
            {
                Student = mapper.Map<StudentDto>(student),
                Hours = reservations.Sum(r => r.Duration / 60.0),
                Profit = reservations.Sum(r => r.Cost)
            };
        }

        public async Task<SubjectReportDto> GetSubjectReportAsync(long subjectId, ReportParameters parameters)
        {
            var subject = await subjectRepository.GetSubjectByIdAsync(subjectId);
            var reservations = await reservationRepository.GetReservationsAsync(GetExpressionToSubjectReservations(subjectId, parameters));

            return new SubjectReportDto
            {
                Subject = mapper.Map<SubjectDto>(subject),
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        public async Task<SubjectCategoryReportDto> GetSubjectCategoryReportAsync(long tutorId, ReportSubjectCategoryParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsAsync(GetExpressionToSubjectCategoryReservations(tutorId, parameters));

            return new SubjectCategoryReportDto
            {
                SubjectCategory = parameters.SubjectCategory,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        public async Task<PlaceReportDto> GetPlaceReportAsync(long tutorId, ReportPlaceParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsAsync(GetExpressionToPlaceReservations(tutorId, parameters));

            return new PlaceReportDto
            {
                Place = parameters.Place,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        private Expression<Func<Reservation, bool>> GetExpressionToTutorReservations(long tutorId, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToStudentReservations(long studentId, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.StudentId.Equals(studentId);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToSubjectReservations(long subjectId, ReportParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.SubjectId.Equals(subjectId);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToSubjectCategoryReservations(long tutorId, ReportSubjectCategoryParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId) && r.Subject.Category.Equals(parameters.SubjectCategory);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);

            return expression;
        }

        private Expression<Func<Reservation, bool>> GetExpressionToPlaceReservations(long tutorId, ReportPlaceParameters parameters)
        {
            Expression<Func<Reservation, bool>> expression = r => r.TutorId.Equals(tutorId) && r.Place.Equals(parameters.Place);
            FilterByStartDate(ref expression, parameters.StartDate);
            FilterByEndDate(ref expression, parameters.EndDate);

            return expression;
        }

        private Expression<Func<AdditionalOrder, bool>> GetExpressionToOrders(long tutorId, ReportParameters parameters)
        {
            Expression<Func<AdditionalOrder, bool>> expression = r => r.TutorId.Equals(tutorId);
            FilterByReceiptStartDate(ref expression, parameters.StartDate);
            FilterByReceiptEndDate(ref expression, parameters.EndDate);

            return expression;
        }

        private void FilterByStartDate(ref Expression<Func<Reservation, bool>> expression, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime.Date >= startDate.Value.Date);
        }

        private void FilterByEndDate(ref Expression<Func<Reservation, bool>> expression, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.StartTime.Date <= endDate.Value.Date);
        }

        private void FilterByReceiptStartDate(ref Expression<Func<AdditionalOrder, bool>> expression, DateTime? startDate)
        {
            if (!startDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.ReceiptDate.Date >= startDate.Value.Date);
        }

        private void FilterByReceiptEndDate(ref Expression<Func<AdditionalOrder, bool>> expression, DateTime? endDate)
        {
            if (!endDate.HasValue)
                return;

            ExpressionMerger.MergeExpression(ref expression, r => r.ReceiptDate.Date <= endDate.Value.Date);
        }
    }
}
