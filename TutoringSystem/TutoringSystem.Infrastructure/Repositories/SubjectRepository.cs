using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddSubjectAsync(Subject subject)
        {
            Create(subject);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteSubjectAsync(Subject subject)
        {
            subject.IsActiv = false;
            Update(subject);

            return await SaveChangedAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(long subjectId)
        {
            var subject = await DbContext.Subjects
                .Include(s => s.Tutor)
                .FirstOrDefaultAsync(s => s.IsActiv && s.Id.Equals(subjectId));

            return subject;
        }

        public async Task<ICollection<Subject>> GetSubjectsByTutorAsync(long tutorId, bool isActiv = true)
        {
            var subjects = await FindByCondition(s => s.IsActiv.Equals(isActiv) && s.TutorId.Equals(tutorId))
                .ToListAsync();

            return subjects;
        }

        public async Task<bool> UpdateSubjectAsync(Subject updatedSubject)
        {
            Update(updatedSubject);

            return await SaveChangedAsync();
        }
    }
}
