using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class StudentTutorRequestRepository : RepositoryBase<StudentTutorRequest>, IStudentTutorRequestRepository
    {
        public StudentTutorRequestRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddRequestAsync(StudentTutorRequest request)
        {
            Create(request);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateRequestAsync(StudentTutorRequest request)
        {
            Update(request);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteRequestAsync(StudentTutorRequest request)
        {
            request.IsActive = false;

            return await UpdateRequestAsync(request);
        }

        public async Task<StudentTutorRequest> GetRequestAsync(Expression<Func<StudentTutorRequest, bool>> expression)
        {
            var request = await DbContext.StudentTutorRequests
                .Include(r => r.Student)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync(expression);

            return request;
        }

        public async Task<IEnumerable<StudentTutorRequest>> GetRequestsCollectionAsync(Expression<Func<StudentTutorRequest, bool>> expression)
        {
            var requests = await FindByCondition(expression)
                .ToListAsync();

            return requests;
        }
    }
}