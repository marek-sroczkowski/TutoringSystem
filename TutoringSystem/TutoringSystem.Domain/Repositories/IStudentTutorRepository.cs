using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IStudentTutorRepository
    {
        Task<StudentTutor> GetStudentTutorAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true);
        Task<IEnumerable<StudentTutor>> GetStudentTuturCollectionAsync(Expression<Func<StudentTutor, bool>> expression, bool? isActive = true);
        Task<bool> UpdateStudentTutorAsync(StudentTutor studentTutor);
    }
}
