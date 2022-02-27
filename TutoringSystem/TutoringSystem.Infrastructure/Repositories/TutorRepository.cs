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
    public class TutorRepository : RepositoryBase<Tutor>, ITutorRepository
    {
        public TutorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddTutorAsync(Tutor tutor)
        {
            Create(tutor);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddTutorsCollectionAsync(IEnumerable<Tutor> tutors)
        {
            CreateRange(tutors);

            return await SaveChangedAsync();
        }

        public async Task<Tutor> GetTutorAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, t => t.IsActive.Equals(isActive.Value));
            }

            var tutor = isEagerLoadingEnabled
                ? await GetTutorWithEagerLoadingAsync(expression)
                : await GetTutorWithoutEagerLoadingAsync(expression);

            return tutor;
        }

        public IQueryable<Tutor> GetTutorsCollection(Expression<Func<Tutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, t => t.IsActive.Equals(isActive.Value));
            }

            var tutors = isEagerLoadingEnabled
                ? GetTutorsCollectionWithEagerLoading(expression)
                : Find(expression);

            return tutors;
        }

        public async Task<IEnumerable<Tutor>> GetTutorsCollectionAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, t => t.IsActive.Equals(isActive.Value));
            }

            var tutors = isEagerLoadingEnabled
                ? GetTutorsCollectionWithEagerLoading(expression)
                : Find(expression);

            return await tutors.ToListAsync();
        }

        public bool IsTutorExist(Expression<Func<Tutor, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, u => u.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveTutorAsync(Tutor tutor)
        {
            tutor.IsActive = false;

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateTutorAsync(Tutor tutor)
        {
            Update(tutor);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateTutorsCollectionAsync(IEnumerable<Tutor> tutors)
        {
            UpdateRange(tutors);

            return await SaveChangedAsync();
        }

        private async Task<Tutor> GetTutorWithEagerLoadingAsync(Expression<Func<Tutor, bool>> expression)
        {
            var tutor = await Find(expression)
                .Include(t => t.Contact)
                .Include(t => t.Address)
                .Include(t => t.PushNotificationToken)
                .FirstOrDefaultAsync();

            return tutor;
        }

        private async Task<Tutor> GetTutorWithoutEagerLoadingAsync(Expression<Func<Tutor, bool>> expression)
        {
            var tutor = await Find(expression)
                .FirstOrDefaultAsync();

            return tutor;
        }

        private IQueryable<Tutor> GetTutorsCollectionWithEagerLoading(Expression<Func<Tutor, bool>> expression)
        {
            var tutors = Find(expression)
                .Include(t => t.Contact)
                .Include(t => t.Address)
                .Include(t => t.PushNotificationToken);

            return tutors;
        }
    }
}