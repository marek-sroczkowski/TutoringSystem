using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IIntervalRepository
    {
        Task<Interval> GetIntervalByIdAsync(long intervalId);
        Task<IEnumerable<Interval>> GetIntervalsAsync(Expression<Func<Interval, bool>> expression);
    }
}
