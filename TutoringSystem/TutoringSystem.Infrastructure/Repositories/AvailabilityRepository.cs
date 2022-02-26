using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> AddAvailabilitiesCollectionAsync(IEnumerable<Availability> availabilities)
        {
            CreateRange(availabilities);

            return await SaveChangedAsync();
        }

        public async Task<Availability> GetAvailabilityAsync(Expression<Func<Availability, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var availability = isEagerLoadingEnabled
                ? await GetAvailabilityWithEagerLoadingAsync(expression)
                : await GetAvailabilityWithoutEagerLoadingAsync(expression);

            return availability;
        }

        public IQueryable<Availability> GetAvailabilitiesCollection(Expression<Func<Availability, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var availabilities = isEagerLoadingEnabled
                ? GetAvailabilitiesCollectionWithEagerLoading(expression)
                : Find(expression);

            return availabilities;
        }

        public bool IsAvailabilityExist(Expression<Func<Availability, bool>> expression)
        {
            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveAvailabilityAsync(Availability availability)
        {
            Delete(availability);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateAvailabilityAsync(Availability availability)
        {
            Update(availability);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateAvailabilitiesCollectionAsync(IEnumerable<Availability> availabilities)
        {
            UpdateRange(availabilities);

            return await SaveChangedAsync();
        }

        private async Task<Availability> GetAvailabilityWithEagerLoadingAsync(Expression<Func<Availability, bool>> expression)
        {
            var availability = await Find(expression)
                .Include(a => a.Intervals)
                .Include(a => a.Tutor)
                .FirstOrDefaultAsync();

            return availability;
        }

        private async Task<Availability> GetAvailabilityWithoutEagerLoadingAsync(Expression<Func<Availability, bool>> expression)
        {
            var availability = await Find(expression)
                .FirstOrDefaultAsync();

            return availability;
        }

        private IQueryable<Availability> GetAvailabilitiesCollectionWithEagerLoading(Expression<Func<Availability, bool>> expression)
        {
            var availabilities = Find(expression)
                .Include(a => a.Intervals)
                .Include(a => a.Tutor);

            return availabilities;
        }
    }
}
