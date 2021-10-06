using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Parameters;
using TutoringSystem.Application.Services.Interfaces;
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
            var students = await studentRepository.GetStudentsAsync();
            students = students.Where(s => s.Tutors.Contains(tutor));
            var reservations = await reservationRepository.GetReservationsByTutorIdAsync(tutorId);
            reservations = reservations.Where(r => r.StartTime.Date >= parameters.StartDate.Date && r.StartTime.Date <= parameters.EndDate.Date);
            var orders = await orderRepository.GetAdditionalOrdersAsync(tutorId);
            orders = orders.Where(o => o.ReceiptDate.Date >= parameters.StartDate.Date && o.ReceiptDate.Date <= parameters.EndDate.Date);

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
            var reservations = await reservationRepository.GetReservationsByStudentIdAsync(studentId);
            reservations = reservations.Where(r => r.StartTime.Date >= parameters.StartDate.Date && r.StartTime.Date <= parameters.EndDate.Date);

            return new StudentSummaryDto
            {
                Student = mapper.Map<StudentDto>(student),
                Hours = reservations.Sum(r => r.Duration / 60.0),
                Profit = reservations.Sum(r => r.Cost)
            };
        }

        public async Task<SubjectReportDto> GetSubjectReportAsync(long tutorId, long subjectId, ReportParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsByTutorIdAsync(tutorId);
            var subject = await subjectRepository.GetSubjectByIdAsync(subjectId);
            reservations = reservations.Where(r => r.SubjectId.Equals(subjectId) && r.StartTime.Date >= parameters.StartDate.Date && r.StartTime.Date <= parameters.EndDate.Date);

            return new SubjectReportDto
            {
                Subject = mapper.Map<SubjectDto>(subject),
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        public async Task<SubjectCategoryReportDto> GetSubjectCategoryReportAsync(long tutorId, ReportSubjectCategoryParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsByTutorIdAsync(tutorId);
            reservations = reservations.Where(r => r.Subject.Category.Equals(parameters.SubjectCategory) && r.StartTime.Date >= parameters.StartDate.Date && r.StartTime.Date <= parameters.EndDate.Date);

            return new SubjectCategoryReportDto
            {
                SubjectCategory = parameters.SubjectCategory,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }

        public async Task<PlaceReportDto> GetPlaceReportAsync(long tutorId, ReportPlaceParameters parameters)
        {
            var reservations = await reservationRepository.GetReservationsByTutorIdAsync(tutorId);
            reservations = reservations.Where(r => r.Place.Equals(parameters.Place) && r.StartTime.Date >= parameters.StartDate.Date && r.StartTime.Date <= parameters.EndDate.Date);

            return new PlaceReportDto
            {
                Place = parameters.Place,
                TotalHours = reservations.Sum(r => r.Duration / 60.0),
                TotalProfit = reservations.Sum(r => r.Cost)
            };
        }
    }
}
