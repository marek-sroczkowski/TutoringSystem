using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.StudentRequest;
using TutoringSystem.Application.Models.Enums;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IStudentRequestService
    {
        Task<AddTutorToStudentStatus> AddRequestAsync(long studentId, long tutorId);
        Task<bool> DeclineRequest(long requestId);
        Task<IEnumerable<StudentRequestDto>> GetRequestsByStudentId(long studentId);
        Task<IEnumerable<StudentRequestDto>> GetRequestsByTutorId(long tutorId);
    }
}
