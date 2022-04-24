using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Student;
using TutoringSystem.Application.Models.Enums;
using TutoringSystem.Application.Models.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<StudentDetailsDto> GetStudentAsync(long tutorId, long studentId);
        Task<IEnumerable<StudentDto>> GetStudentsByTutorIdAsync(long tutorId);
        Task<bool> RemoveStudentAsync(long tutorId, long studentId);
        Task<AddStudentToTutorStatus> AddStudentToTutorAsync(long tutorId, NewExistingStudentDto student);
        Task<bool> UpdateStudentAsync(long tutorId, UpdatedStudentDto student);
        Task<PagedList<StudentSimpleDto>> GetStudents(SearchedUserParameters parameters);
    }
}