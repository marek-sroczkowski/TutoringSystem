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

        public async Task<bool> AddStudentsCollectionAsync(IEnumerable<Student> students)
        {
            CreateRange(students);

            return await SaveChangedAsync();
        }

        public async Task<Student> GetStudentAsync(Expression<Func<Student, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var student = isEagerLoadingEnabled
                ? await GetStudentWithEagerLoadingAsync(expression)
                : await GetStudentWithoutEagerLoadingAsync(expression);

            return student;
        }

        public IQueryable<Student> GetStudentsCollection(Expression<Func<Student, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var students = isEagerLoadingEnabled
                ? GetStudentsCollectionWithEagerLoading(expression)
                : Find(expression);

            return students;
        }

        public async Task<IEnumerable<Student>> GetStudentsCollectionAsync(Expression<Func<Student, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var students = isEagerLoadingEnabled
                ? GetStudentsCollectionWithEagerLoading(expression)
                : Find(expression);

            return await students.ToListAsync();
        }

        public bool IsStudentExist(Expression<Func<Student, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveStudentAsync(Student student)
        {
            student.IsActive = false;

            return await UpdateStudentAsync(student);
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            Update(student);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateStudentsCollectionAsync(IEnumerable<Student> students)
        {
            UpdateRange(students);

            return await SaveChangedAsync();
        }

        private async Task<Student> GetStudentWithEagerLoadingAsync(Expression<Func<Student, bool>> expression)
        {
            var student = await Find(expression)
                .Include(s => s.StudentTutors)
                .Include(s => s.Tutors)
                .Include(s => s.Address)
                .Include(s => s.Contact)
                .FirstOrDefaultAsync();

            return student;
        }

        private async Task<Student> GetStudentWithoutEagerLoadingAsync(Expression<Func<Student, bool>> expression)
        {
            var student = await Find(expression)
                .FirstOrDefaultAsync();

            return student;
        }

        private IQueryable<Student> GetStudentsCollectionWithEagerLoading(Expression<Func<Student, bool>> expression)
        {
            var students = Find(expression)
                .Include(s => s.StudentTutors)
                .Include(s => s.Tutors)
                .Include(s => s.Address)
                .Include(s => s.Contact);

            return students;
        }
    }
}
