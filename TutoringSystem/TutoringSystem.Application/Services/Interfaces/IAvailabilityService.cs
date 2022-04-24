using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Availability;
using TutoringSystem.Application.Models.Parameters;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<AvailabilityDto> AddAvailabilityAsync(long tutorId, NewAvailabilityDto newAvailability);
        Task<AvailabilityDetailsDto> GetAvailabilityByIdAsync(long availabilityId);
        Task<AvailabilityDetailsDto> GetTodaysAvailabilityByTutorAsync(long tutorId);
        Task<PagedList<AvailabilityDto>> GetAvailabilitiesByTutorAsync(long tutorId, AvailabilityParameters parameters);
        Task<PagedList<AvailabilityDto>> GetFutureAvailabilitiesByTutorAsync(long tutorId, FutureAvailabilityParameters parameters);
        Task<bool> UpdateAvailabilityAsync(UpdatedAvailabilityDto updatedAvailability);
        Task<bool> RemoveAvailabilityAsync(long availabilityId);
    }
}
