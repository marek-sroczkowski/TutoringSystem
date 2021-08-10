using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext dbContext;

        public StudentRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddStudentAsycn(Student student)
        {
            await dbContext.Students.AddAsync(student);
            return (await dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<Student> GetStudentByIdAsync(long id)
        {
            var student = await dbContext.Students
                .Where(s => s.IsActiv)
                .FirstOrDefaultAsync(s => s.Id.Equals(id));

            return student;
        }

        public async Task<ICollection<Student>> GetStudentsAsync() => await dbContext.Students
            .Where(s => s.IsActiv)
            .ToListAsync();

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            dbContext.Students.Update(student);
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
