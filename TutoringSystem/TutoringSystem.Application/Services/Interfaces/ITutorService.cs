using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.TutorDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface ITutorService
    {
        Task<ICollection<TutorDto>> GetTutorsByStudentIdAsync(long studentId);
        Task<TutorDetailsDto> GetTutorAsync(long tutorId, long studentId);
        Task<bool> RemoveTutorAsync(long studentId, long tutorId);
        Task<bool> RemoveAllTutorsAsync(long studentId);
        Task<bool> AddTutorToStudentAsync(long studentId, long tutorId);
    }
}
