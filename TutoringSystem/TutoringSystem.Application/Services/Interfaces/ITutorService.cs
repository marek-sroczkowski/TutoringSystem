using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Tutor;
using TutoringSystem.Application.Models.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface ITutorService
    {
        Task<IEnumerable<TutorDto>> GetTutorsByStudentIdAsync(long studentId);
        Task<TutorDetailsDto> GetTutorAsync(long tutorId, long studentId);
        Task<bool> RemoveTutorAsync(long studentId, long tutorId);
        Task<PagedList<TutorSimpleDto>> GetTutors(SearchedUserParameters parameters);
    }
}