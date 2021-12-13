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

        public async Task<StudentTutor> GetStudentTutorAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));

            var result = await DbContext.StudentTutors
                .Include(st => st.Student)
                .Include(st => st.Tutor)
                .FirstOrDefaultAsync(expression);

            return result;
        }

        public async Task<IEnumerable<StudentTutor>> GetStudentTuturCollectionAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, st => st.IsActive.Equals(isActive.Value));

            var result = await FindByCondition(expression)
                .Include(st => st.Student)
                .Include(st => st.Tutor)
                .ToListAsync();

            return result;
        }

        public async Task<bool> UpdateStudentTutorAsync(StudentTutor studentTutor)
        {
            Update(studentTutor);

            return await SaveChangedAsync();
        }
    }
}
