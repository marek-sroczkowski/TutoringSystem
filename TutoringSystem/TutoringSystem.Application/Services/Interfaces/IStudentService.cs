using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDetailsDto> GetStudentAsync(long tutorId, long studentId);
        Task<IEnumerable<StudentDto>> GetStudentsByTutorIdAsync(long tutorId);
        Task<bool> RemoveStudentAsync(long tutorId, long studentId);
        Task<bool> RemoveAllStudentsAsync(long tutorId);
        Task<AddStudentToTutorStatus> AddStudentToTutorAsync(long tutorId, NewExistingStudentDto student);
        Task<bool> UpdateStudentAsync(long tutorId, UpdatedStudentDto student);
        Task<PagedList<StudentSimpleDto>> GetStudents(SearchedUserParameters parameters);
    }
}