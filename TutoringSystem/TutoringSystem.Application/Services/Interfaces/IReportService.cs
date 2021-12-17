using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReportDtos;
using TutoringSystem.Application.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IReportService
    {
        IEnumerable<PlaceReportDto> GetPlacesReport(long tutorId, ReportParameters parameters);
        TutorReportDto GetGeneralReport(long tutorId, ReportParameters parameters);
        Task<IEnumerable<StudentReportDto>> GetStudentsReportAsync(long tutorId, ReportParameters parameters);
        IEnumerable<SubjectCategoryReportDto> GetSubjectCategoriesReport(long tutorId, ReportParameters parameters);
        Task<IEnumerable<SubjectReportDto>> GetSubjectsReportAsync(long tutorId, ReportParameters parameters);
        IEnumerable<GeneralTimedReportDto> GetGeneralTimedReport(long tutorId, ReportParameters parameters);
    }
}