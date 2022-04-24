using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<bool> AddAvailabilityAsync(Availability availability);
        Task<bool> AddAvailabilitiesCollectionAsync(IEnumerable<Availability> availabilities);
        Task<Availability> GetAvailabilityAsync(Expression<Func<Availability, bool>> expression, bool isEagerLoadingEnabled = false);
        IQueryable<Availability> GetAvailabilitiesCollection(Expression<Func<Availability, bool>> expression, bool isEagerLoadingEnabled = false);
        bool AvailabilityExists(Expression<Func<Availability, bool>> expression);
        Task<bool> RemoveAvailabilityAsync(Availability availability);
        Task<bool> UpdateAvailabilityAsync(Availability updatedAvailability);
        Task<bool> UpdateAvailabilitiesCollectionAsync(IEnumerable<Availability> availabilities);
        Task<IEnumerable<Availability>> GetAvailabilitiesCollectionAsync(Expression<Func<Availability, bool>> expression, bool isEagerLoadingEnabled = false);
    }
}