using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;

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