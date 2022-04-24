using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IStudentRepository
    {
        Task<bool> AddStudentAsync(Student student);
        Task<bool> AddStudentsCollectionAsync(IEnumerable<Student> students);
        Task<Student> GetStudentAsync(Expression<Func<Student, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<Student> GetStudentsCollection(Expression<Func<Student, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<Student>> GetStudentsCollectionAsync(Expression<Func<Student, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool StudentExists(Expression<Func<Student, bool>> expression, bool? isActive = true);
        Task<bool> RemoveStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> UpdateStudentsCollectionAsync(IEnumerable<Student> students);
    }
}