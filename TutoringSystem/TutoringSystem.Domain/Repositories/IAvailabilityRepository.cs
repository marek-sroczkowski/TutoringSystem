using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<bool> AddAvailabilityAsync(Availability availability);
        Task<bool> UpdateAvailabilityAsync(Availability updatedAvailability);
        Task<bool> DeleteAvailabilityAsync(Availability availability);
        Task<Availability> GetAvailabilityAsync(Expression<Func<Availability, bool>> expression);
        Task<IEnumerable<Availability>> GetAvailabilitiesCollectionAsync(Expression<Func<Availability, bool>> expression);
    }
}
