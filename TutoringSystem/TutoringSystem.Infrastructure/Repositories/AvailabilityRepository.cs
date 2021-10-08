using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<Availability> GetAvailabilityAsync(Expression<Func<Availability, bool>> expression)
        {
            var availability = await DbContext.Availabilities
               .Include(a => a.Intervals)
               .Include(a => a.Tutor)
               .FirstOrDefaultAsync(expression);

            return availability;
        }

        public async Task<IEnumerable<Availability>> GetAvailabilitiesCollectionAsync(Expression<Func<Availability, bool>> expression)
        {
            var availabilities = await FindByCondition(expression)
                .Include(a => a.Intervals)
                .Include(a => a.Tutor)
                .ToListAsync();

            return availabilities;
        }

        public async Task<bool> UpdateAvailabilityAsync(Availability updatedAvailability)
        {
            Update(updatedAvailability);

            return await SaveChangedAsync();
        }
    }
}
