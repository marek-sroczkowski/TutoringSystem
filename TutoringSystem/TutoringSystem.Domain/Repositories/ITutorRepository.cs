using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface ITutorRepository
    {
        Task<bool> AddTutorAsync(Tutor tutor);
        Task<Tutor> GetTutorByIdAsync(long id);
        Task<ICollection<Tutor>> GetAllTutorsAsync();
        Task<bool> UpdateTutorAsync(Tutor tutor);
    }
}
