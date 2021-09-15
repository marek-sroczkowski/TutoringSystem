using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class TutorRepository : RepositoryBase<Tutor>, ITutorRepository
    {
        public TutorRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddTutorAsync(Tutor tutor)
        {
            Create(tutor);

            return await SaveChangedAsync();
        }

        public async Task<ICollection<Tutor>> GetAllTutorsAsync()
        {
            var tutors = await FindByCondition(t => t.IsActiv)
                            .ToListAsync();

            return tutors;
        }

        public async Task<Tutor> GetTutorByIdAsync(long tutorId)
        {
            var tutor = await DbContext.Tutors
                .Where(t => t.IsActiv)
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(t => t.Id.Equals(tutorId));

            return tutor;
        }

        public async Task<bool> UpdateTutorAsync(Tutor tutor)
        {
            Update(tutor);

            return await SaveChangedAsync();
        }
    }
}
