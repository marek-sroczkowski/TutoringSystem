using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IStudentTutorRequestRepository
    {
        Task<bool> AddRequestAsync(StudentTutorRequest request);
        Task<bool> DeleteRequestAsync(StudentTutorRequest request);
        Task<StudentTutorRequest> GetRequestAsync(Expression<Func<StudentTutorRequest, bool>> expression);
        Task<IEnumerable<StudentTutorRequest>> GetRequestsCollectionAsync(Expression<Func<StudentTutorRequest, bool>> expression);
        Task<bool> UpdateRequestAsync(StudentTutorRequest request);
    }
}
