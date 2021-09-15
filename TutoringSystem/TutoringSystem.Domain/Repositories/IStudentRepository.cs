using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IStudentRepository
    {
        Task<bool> AddStudentAsycn(Student student);
        Task<Student> GetStudentByIdAsync(long studentId);
        Task<ICollection<Student>> GetStudentsAsync();
        Task<bool> UpdateStudentAsync(Student student);
    }
}
