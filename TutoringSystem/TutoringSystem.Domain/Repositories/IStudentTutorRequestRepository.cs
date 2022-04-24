using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IStudentTutorRequestRepository
    {
        Task<bool> AddRequestAsync(StudentTutorRequest request);
        Task<bool> AddRequestsCollectionAsync(IEnumerable<StudentTutorRequest> requests);
        Task<StudentTutorRequest> GetRequestAsync(Expression<Func<StudentTutorRequest, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<StudentTutorRequest> GetRequestsCollection(Expression<Func<StudentTutorRequest, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<StudentTutorRequest>> GetRequestsCollectionAsync(Expression<Func<StudentTutorRequest, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool RequestExists(Expression<Func<StudentTutorRequest, bool>> expression, bool? isActive = true);
        Task<bool> RemoveRequestAsync(StudentTutorRequest request);
        Task<bool> UpdateRequestAsync(StudentTutorRequest request);
        Task<bool> UpdateRequestsCollectionAsync(IEnumerable<StudentTutorRequest> requests);
    }
}