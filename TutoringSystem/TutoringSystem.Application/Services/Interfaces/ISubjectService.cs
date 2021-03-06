using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.SubjectDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectDto> AddSubjectAsync(long tutorId, NewSubjectDto newSubjectModel);
        Task<bool> DeactivateSubjectAsync(long subjectId);
        Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId);
        Task<IEnumerable<SubjectDto>> GetTutorSubjectsAsync(long tutorId);
        Task<bool> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject);
    }
}