using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class AvailabilityRepository : RepositoryBase<Availability>, IAvailabilityRepository
    {
        public AvailabilityRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddAvailabilityAsync(Availability availability)
        {
            Create(availability);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteAvailabilityAsync(Availability availability)
        {
            Delete(availability);

            return await SaveChangedAsync();
        }

        public async Task<ICollection<Availability>> GetAvailabilitiesByTutorIdAsync(long tutorId)
        {
            var availabilities = await FindByCondition(a => a.TutorId.Equals(tutorId))
                .Include(a => a.Intervals)
                .Include(a => a.Tutor)
                .ToListAsync();

            return availabilities;
        }

        public async Task<Availability> GetAvailabilityByIdAsync(long availabilityId)
        {
            var availability = await DbContext.Availabilities
               .Include(a => a.Intervals)
               .Include(a => a.Tutor)
               .FirstOrDefaultAsync(a => a.Id.Equals(availabilityId));

            return availability;
        }

        public async Task<ICollection<Availability>> GetFutureAvailabilitiesByTutorIdAsync(long tutorId)
        {
            var availabilities = await FindByCondition(a => a.Date > DateTime.Now && a.TutorId.Equals(tutorId))
                .Include(a => a.Intervals)
                .Include(a => a.Tutor)
                .ToListAsync();

            return availabilities;
        }

        public async Task<Availability> GetTodaysAvailabilityByTutorIdAsync(long tutorId)
        {
            var availability = await DbContext.Availabilities
                .Include(a => a.Intervals)
                .Include(a => a.Tutor)
                .FirstOrDefaultAsync(a => a.Date.Date.Equals(DateTime.Now.Date) && a.TutorId.Equals(tutorId));

            return availability;
        }

        public async Task<bool> UpdateAvailabilityAsync(Availability updatedAvailability)
        {
            Update(updatedAvailability);

            return await SaveChangedAsync();
        }
    }
}
