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

        public async Task<Tutor> GetTutorAsync(Expression<Func<Tutor, bool>> expression, bool? isActiv = true)
        {
            if (isActiv.HasValue)
                ExpressionMerger.MergeExpression(ref expression, t => t.IsActiv.Equals(isActiv.Value));

            var tutor = await DbContext.Tutors
                .Where(t => t.IsActiv.Equals(isActiv))
                .Include(t => t.Subjects)
                .Include(t => t.Students)
                .FirstOrDefaultAsync(expression);

            return tutor;
        }

        public async Task<IEnumerable<Tutor>> GetTutorsCollectionAsync(Expression<Func<Tutor, bool>> expression, bool? isActiv = true)
        {
            if (isActiv.HasValue)
                ExpressionMerger.MergeExpression(ref expression, t => t.IsActiv.Equals(isActiv.Value));

            var tutors = await FindByCondition(expression)
                            .ToListAsync();

            return tutors;
        }

        public async Task<bool> UpdateTutorAsync(Tutor tutor)
        {
            Update(tutor);

            return await SaveChangedAsync();
        }
    }
}
