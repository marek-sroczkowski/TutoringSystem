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
        Task<Availability> GetAvailabilityByIdAsync(long availabilityId);
        Task<Availability> GetTodaysAvailabilityByTutorIdAsync(long tutorId);
        Task<Availability> GetAvailabilityByTutorIdAndDateAsync(long tutorId, DateTime date);
        Task<bool> UpdateAvailabilityAsync(Availability updatedAvailability);
        Task<bool> DeleteAvailabilityAsync(Availability availability);
        Task<IEnumerable<Availability>> GetAvailabilitiesAsync(Expression<Func<Availability, bool>> expression);
    }
}
