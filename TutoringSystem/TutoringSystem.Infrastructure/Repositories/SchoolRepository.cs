using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class SchoolRepository : RepositoryBase<School>, ISchoolRepository
    {
        public SchoolRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddSchoolAsync(School school)
        {
            Create(school);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteSchoolAsync(School school)
        {
            Delete(school);

            return await SaveChangedAsync();
        }

        public async Task<School> GetSchoolByIdAsync(int schoolId)
        {
            var school = await DbContext.Schools
                .Include(s => s.Student)
                .FirstOrDefaultAsync(s => s.Id.Equals(schoolId));

            return school;
        }

        public async Task<bool> UpdateSchoolAsync(School updatedSchool)
        {
            Update(updatedSchool);

            return await SaveChangedAsync();
        }
    }
}
