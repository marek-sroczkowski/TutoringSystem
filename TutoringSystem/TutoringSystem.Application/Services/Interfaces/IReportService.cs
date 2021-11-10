using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<PlaceReportDto> GetPlaceReportAsync(long tutorId, ReportPlaceParameters parameters);
        Task<TutorReportDto> GetReportByTutorAsync(long tutorId, ReportParameters parameters);
        Task<StudentSummaryDto> GetStudentSummaryAsync(long studentId, long tutorId, ReportParameters parameters);
        Task<SubjectCategoryReportDto> GetSubjectCategoryReportAsync(long tutorId, ReportSubjectCategoryParameters parameters);
        Task<SubjectReportDto> GetSubjectReportAsync(long subjectId, ReportParameters parameters);
    }
}
