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
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddStudentAsync(Student student)
        {
            Create(student);

            return await SaveChangedAsync();
        }

        public async Task<Student> GetStudentAsync(Expression<Func<Student, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));

            var student = await DbContext.Students
                .Include(s => s.StudentTutors)
                .Include(s => s.Tutors)
                .FirstOrDefaultAsync(expression);

            return student;
        }

        public async Task<IEnumerable<Student>> GetStudentsCollectionAsync(Expression<Func<Student, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));

            var students = await FindByCondition(expression)
                .Include(s => s.StudentTutors)
                .ToListAsync();

            return students;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            Update(student);

            return await SaveChangedAsync();
        }
    }
}
