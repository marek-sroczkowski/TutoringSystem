using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<Tutor> GetTutorAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, t => t.IsActive.Equals(isActive.Value));

            var tutor = await DbContext.Tutors
                .Include(t => t.Contact)
                .Include(t => t.Address)
                .Include(t => t.StudentTutors)
                .FirstOrDefaultAsync(expression);

            return tutor;
        }

        public async Task<IEnumerable<Tutor>> GetTutorsCollectionAsync(Expression<Func<Tutor, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, t => t.IsActive.Equals(isActive.Value));

            var tutors = await FindByCondition(expression)
                .Include(t => t.Contact)
                .Include(t => t.Address)
                .ToListAsync();

            return tutors;
        }

        public async Task<bool> UpdateTutorAsync(Tutor tutor)
        {
            Update(tutor);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateTutorsCollection(IEnumerable<Tutor> tutors)
        {
            DbContext.UpdateRange(tutors);

            return await SaveChangedAsync();
        }
    }
}