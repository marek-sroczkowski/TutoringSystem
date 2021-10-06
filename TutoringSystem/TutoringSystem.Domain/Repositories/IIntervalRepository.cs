using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IIntervalRepository
    {
        Task<Interval> GetIntervalByIdAsync(long intervalId);
    }
}
