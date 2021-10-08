using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ISubjectRepository
    {
        Task<bool> AddSubjectAsync(Subject subject);
        Task<Subject> GetSubjectByIdAsync(long subjectId);
        Task<bool> UpdateSubjectAsync(Subject updatedSubject);
        Task<bool> DeleteSubjectAsync(Subject subject);
        Task<IEnumerable<Subject>> GetSubjectsAsync(Expression<Func<Subject, bool>> expression, bool? isActiv = true);
    }
}
