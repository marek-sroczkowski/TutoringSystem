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
    public class StudentTutorRepository : RepositoryBase<StudentTutor>, IStudentTutorRepository
    {
        public StudentTutorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddStudentTutorAsync(StudentTutor studentTutor)
        {
            Create(studentTutor);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddStudentTutorsCollectionAsync(IEnumerable<StudentTutor> studentTutors)
        {
            CreateRange(studentTutors);

            return await SaveChangedAsync();
        }

        public async Task<StudentTutor> GetStudentTutorAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var result = isEagerLoadingEnabled
                ? await GetObjectWithEagerLoadingAsync(expression)
                : await GetObjectWithoutEagerLoadingAsync(expression);

            return result;
        }

        public IQueryable<StudentTutor> GetStudentTuturCollection(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, st => st.IsActive.Equals(isActive.Value));
            }

            var result = isEagerLoadingEnabled
                ? GetCollectionWithEagerLoading(expression)
                : Find(expression);

            return result;
        }

        public async Task<IEnumerable<StudentTutor>> GetStudentTutorCollectionAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, st => st.IsActive.Equals(isActive.Value));
            }

            var result = isEagerLoadingEnabled
                ? GetCollectionWithEagerLoading(expression)
                : Find(expression);

            return await result.ToListAsync();
        }

        public bool StudentTutorExists(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, st => st.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveStudentTutorAsync(StudentTutor studentTutor)
        {
            studentTutor.IsActive = false;

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateStudentTutorAsync(StudentTutor studentTutor)
        {
            Update(studentTutor);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateStudentTutorsCollectionAsync(IEnumerable<StudentTutor> studentTutors)
        {
            UpdateRange(studentTutors);

            return await SaveChangedAsync();
        }

        private async Task<StudentTutor> GetObjectWithEagerLoadingAsync(Expression<Func<StudentTutor, bool>> expression)
        {
            var studentTutor = await Find(expression)
                .Include(st => st.Student)
                .Include(st => st.Tutor)
                .FirstOrDefaultAsync();

            return studentTutor;
        }

        private async Task<StudentTutor> GetObjectWithoutEagerLoadingAsync(Expression<Func<StudentTutor, bool>> expression)
        {
            var studentTutor = await Find(expression)
                .FirstOrDefaultAsync();

            return studentTutor;
        }

        private IQueryable<StudentTutor> GetCollectionWithEagerLoading(Expression<Func<StudentTutor, bool>> expression)
        {
            var studentTutors = Find(expression)
                .Include(st => st.Student)
                .Include(st => st.Tutor);

            return studentTutors;
        }
    }
}
