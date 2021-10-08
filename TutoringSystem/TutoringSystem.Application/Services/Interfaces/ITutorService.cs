using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.TutorDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface ITutorService
    {
        Task<bool> AddStudentAsync(long tutorId, long studentId);
        Task<ICollection<StudentDto>> GetStudentsAsync(long tutorId);
        Task<TutorDetailsDto> GetTutorAsync(long tutorId);
        Task<bool> RemoveAllStudentsAsync(long tutorId);
        Task<bool> RemoveStudentAsync(long tutorId, long studentId);
    }
}
