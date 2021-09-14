using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<bool> AddStudentAsycn(Student student)
        {
            Create(student);

            return await SaveChangedAsync();
        }

        public async Task<ICollection<Student>> GetStudentsAsync()
        {
            var students = await FindByCondition(s => s.IsActiv)
                .ToListAsync();

            return students;
        }

        public async Task<Student> GetStudentByIdAsync(long id)
        {
            var student = await DbContext.Students
                .Where(s => s.IsActiv)
                .FirstOrDefaultAsync(s => s.Id.Equals(id));

            return student;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            Update(student);

            return await SaveChangedAsync();
        }
    }
}
