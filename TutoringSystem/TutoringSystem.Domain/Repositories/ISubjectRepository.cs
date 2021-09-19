using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ISubjectRepository
    {
        Task<bool> AddSubjectAsync(Subject subject);
        Task<Subject> GetSubjectByIdAsync(long subjectId);
        Task<ICollection<Subject>> GetSubjectsByTutorAsync(long tutorId, bool isActiv = true);
        Task<bool> UpdateSubjectAsync(Subject updatedSubject);
        Task<bool> DeleteSubjectAsync(Subject subject);
    }
}
