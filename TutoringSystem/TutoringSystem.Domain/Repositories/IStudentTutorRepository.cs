using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IStudentTutorRepository
    {
        Task<bool> AddStudentTutorAsync(StudentTutor studentTutor);
        Task<bool> AddStudentTutorsCollectionAsync(IEnumerable<StudentTutor> studentTutors);
        Task<StudentTutor> GetStudentTutorAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<StudentTutor> GetStudentTuturCollection(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<StudentTutor>> GetStudentTuturCollectionAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool IsStudentTutorExist(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true);
        Task<bool> RemoveStudentTutorAsync(StudentTutor studentTutor);
        Task<bool> UpdateStudentTutorAsync(StudentTutor studentTutor);
        Task<bool> UpdateStudentTutorsCollectionAsync(IEnumerable<StudentTutor> studentTutors);
    }
}