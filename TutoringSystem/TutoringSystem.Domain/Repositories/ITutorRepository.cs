using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ITutorRepository
    {
        Task<bool> AddTutorAsync(Tutor tutor);
        Task<bool> UpdateTutorAsync(Tutor tutor);
        Task<Tutor> GetTutorAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true);
        Task<IEnumerable<Tutor>> GetTutorsCollectionAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true);
    }
}
