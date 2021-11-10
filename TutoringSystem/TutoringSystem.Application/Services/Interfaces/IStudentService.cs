using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDetailsDto> GetStudentAsync(long tutorId, long studentId);
        Task<ICollection<StudentDto>> GetStudentsByTutorIdAsync(long tutorId);
        Task<bool> RemoveStudentAsync(long tutorId, long studentId);
        Task<bool> RemoveAllStudentsAsync(long tutorId);
        Task<AddStudentToTutorStatus> AddStudentToTutorAsync(long tutorId, NewTutorsStudentDto student);
    }
}