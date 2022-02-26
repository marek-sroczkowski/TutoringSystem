using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
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

        public async Task<bool> AddRequestsCollectionAsync(IEnumerable<StudentTutorRequest> requests)
        {
            CreateRange(requests);

            return await SaveChangedAsync();
        }

        public async Task<StudentTutorRequest> GetRequestAsync(Expression<Func<StudentTutorRequest, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var request = isEagerLoadingEnabled
                ? await GetRequestWithEagerLoadingAsync(expression)
                : await GetRequestWithoutEagerLoadingAsync(expression);

            return request;
        }

        public IQueryable<StudentTutorRequest> GetRequestsCollection(Expression<Func<StudentTutorRequest, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var requests = isEagerLoadingEnabled
                ? GetRequestsCollectionWithEagerLoading(expression)
                : Find(expression);

            return requests;
        }

        public bool IsRequestExist(Expression<Func<StudentTutorRequest, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, o => o.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveRequestAsync(StudentTutorRequest request)
        {
            request.IsActive = false;

            return await UpdateRequestAsync(request);
        }

        public async Task<bool> UpdateRequestAsync(StudentTutorRequest request)
        {
            Update(request);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateRequestsCollectionAsync(IEnumerable<StudentTutorRequest> requests)
        {
            UpdateRange(requests);

            return await SaveChangedAsync();
        }

        private async Task<StudentTutorRequest> GetRequestWithEagerLoadingAsync(Expression<Func<StudentTutorRequest, bool>> expression)
        {
            var request = await Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Tutor)
                .FirstOrDefaultAsync();

            return request;
        }

        private async Task<StudentTutorRequest> GetRequestWithoutEagerLoadingAsync(Expression<Func<StudentTutorRequest, bool>> expression)
        {
            var request = await Find(expression)
                .FirstOrDefaultAsync();

            return request;
        }

        private IQueryable<StudentTutorRequest> GetRequestsCollectionWithEagerLoading(Expression<Func<StudentTutorRequest, bool>> expression)
        {
            var requests = Find(expression)
                .Include(r => r.Student)
                .Include(r => r.Tutor);

            return requests;
        }
    }
}