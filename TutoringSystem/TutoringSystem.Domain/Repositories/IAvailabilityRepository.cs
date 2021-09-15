using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<bool> AddAvailabilityAsync(Availability availability);
        Task<Availability> GetAvailabilityByIdAsync(long availabilityId);
        Task<Availability> GetTodaysAvailabilityByTutorIdAsync(long tutorId);
        Task<ICollection<Availability>> GetAvailabilitiesByTutorIdAsync(long tutorId);
        Task<ICollection<Availability>> GetFutureAvailabilitiesByTutorIdAsync(long tutorId);
        Task<bool> UpdateAvailabilityAsync(Availability updatedAvailability);
        Task<bool> DeleteAvailabilityAsync(Availability availability);
    }
}
