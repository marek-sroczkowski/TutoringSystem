using System.Threading.Tasks;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IStudentTutorRequestNotificationService
    {
        Task<bool> SendNotificationToTutorDevice(long studentId, long tutorId);
    }
}