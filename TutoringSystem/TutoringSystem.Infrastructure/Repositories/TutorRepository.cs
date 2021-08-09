using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly AppDbContext dbContext;

        public TutorRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddTutorAsync(Tutor tutor)
        {
            await dbContext.Tutors.AddAsync(tutor);
            return (await dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<ICollection<Tutor>> GetAllTutorsAsync() => await dbContext.Tutors
            .Where(s => s.IsActiv)
            .ToListAsync();

        public async Task<Tutor> GetTutorByIdAsync(long id)
        {
            var tutor = await dbContext.Tutors
                .Where(s => s.IsActiv)
                .FirstOrDefaultAsync(s => s.Id.Equals(id));

            return tutor;
        }

        public async Task<bool> UpdateTutorAsync(Tutor tutor)
        {
            dbContext.Tutors.Update(tutor);
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
