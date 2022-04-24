using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ISubjectRepository
    {
        Task<bool> AddSubjectAsync(Subject subject);
        Task<bool> AddSubjectsCollection(IEnumerable<Subject> subjects);
        Task<Subject> GetSubjectAsync(Expression<Func<Subject, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<Subject> GetSubjectsCollection(Expression<Func<Subject, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<Subject>> GetSubjectsCollectionAsync(Expression<Func<Subject, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool SubjectExists(Expression<Func<Subject, bool>> expression, bool? isActive = true);
        Task<bool> RemoveSubjectAsync(Subject subject);
        Task<bool> UpdateSubjectAsync(Subject updatedSubject);
        Task<bool> UpdateSubjectsCollectionAsync(IEnumerable<Subject> subjects);
    }
}