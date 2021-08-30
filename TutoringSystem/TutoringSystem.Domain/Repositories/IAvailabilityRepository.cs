using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<bool> AddAvailabilityAsync(Availability availability);
        Task<Availability> GetAvailabilityByIdAsync(int availabilityId);
        Task<Availability> GetTodaysAvailabilityByTutorIdAsync(int tutorId);
        Task<ICollection<Availability>> GetAvailabilitiesByTutorIdAsync(int tutorId);
        Task<ICollection<Availability>> GetFutureAvailabilitiesByTutorIdAsync(int tutorId);
        Task<bool> UpdateAvailabilityAsync(Availability updatedAvailability);
        Task<bool> DeleteAvailabilityAsync(Availability availability);
    }
}
