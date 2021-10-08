using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.TutorDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<bool> AddTutorAsync(long studentId, long tutorId);
        Task<StudentDetailsDto> GetStudentAsync(long studentId);
        Task<ICollection<TutorDto>> GetTutorsAsync(long studentId);
        Task<bool> RemoveAllTutorsAsync(long studentId);
        Task<bool> RemoveTutorAsync(long studentId, long tutorId);
    }
}