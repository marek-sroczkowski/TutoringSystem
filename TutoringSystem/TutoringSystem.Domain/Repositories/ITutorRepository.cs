using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ITutorRepository
    {
        Task<bool> AddTutorAsync(Tutor tutor);
        Task<bool> AddTutorsCollectionAsync(IEnumerable<Tutor> tutors);
        Task<Tutor> GetTutorAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<Tutor> GetTutorsCollection(Expression<Func<Tutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<Tutor>> GetTutorsCollectionAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool TutorExists(Expression<Func<Tutor, bool>> expression, bool? isActive = true);
        Task<bool> RemoveTutorAsync(Tutor tutor);
        Task<bool> UpdateTutorAsync(Tutor tutor);
        Task<bool> UpdateTutorsCollectionAsync(IEnumerable<Tutor> tutors);
    }
}