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
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddSubjectAsync(Subject subject)
        {
            Create(subject);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddSubjectsCollection(IEnumerable<Subject> subjects)
        {
            CreateRange(subjects);

            return await SaveChangedAsync();
        }

        public async Task<Subject> GetSubjectAsync(Expression<Func<Subject, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var subject = isEagerLoadingEnabled
                ? await GetSubjectWithEagerLoadingAsync(expression)
                : await GetSubjectWithoutEagerLoadingAsync(expression);

            return subject;
        }

        public IQueryable<Subject> GetSubjectsCollection(Expression<Func<Subject, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var subjects = isEagerLoadingEnabled
                ? GetSubjectsCollectionWithEagerLoading(expression)
                : Find(expression);

            return subjects;
        }

        public bool IsSubjectExist(Expression<Func<Subject, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveSubjectAsync(Subject subject)
        {
            subject.IsActive = false;

            return await UpdateSubjectAsync(subject);
        }

        public async Task<bool> UpdateSubjectAsync(Subject subject)
        {
            Update(subject);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateSubjectsCollectionAsync(IEnumerable<Subject> subjects)
        {
            UpdateRange(subjects);

            return await SaveChangedAsync();
        }

        private async Task<Subject> GetSubjectWithEagerLoadingAsync(Expression<Func<Subject, bool>> expression)
        {
            var subject = await Find(expression)
                .Include(o => o.Tutor)
                .FirstOrDefaultAsync();

            return subject;
        }

        private async Task<Subject> GetSubjectWithoutEagerLoadingAsync(Expression<Func<Subject, bool>> expression)
        {
            var subject = await Find(expression)
                .FirstOrDefaultAsync();

            return subject;
        }

        private IQueryable<Subject> GetSubjectsCollectionWithEagerLoading(Expression<Func<Subject, bool>> expression)
        {
            var subjects = Find(expression)
                .Include(o => o.Tutor);

            return subjects;
        }
    }
}