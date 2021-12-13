using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.StudentRequestDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IStudentRequestService
    {
        Task<bool> AcceptRequest(long requestId, NewExistingStudentDto student);
        Task<AddTutorToStudentStatus> AddRequestAsync(long studentId, long tutorId);
        Task<bool> DeclineRequest(long requestId);
        Task<IEnumerable<StudentRequestDto>> GetRequestsByStudentId(long studentId);
        Task<IEnumerable<StudentRequestDto>> GetRequestsByTutorId(long tutorId);
    }
}
