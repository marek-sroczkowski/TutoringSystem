using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IIntervalRepository
    {
        Task<Interval> GetIntervalAsync(Expression<Func<Interval, bool>> expression);
        Task<IEnumerable<Interval>> GetIntervalsCollectionAsync(Expression<Func<Interval, bool>> expression);
    }
}
