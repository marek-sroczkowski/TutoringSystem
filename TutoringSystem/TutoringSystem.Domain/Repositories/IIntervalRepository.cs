using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IIntervalRepository
    {
        Task<bool> AddIntervalAsync(Interval interval);
        Task<bool> AddIntervalsCollectionAsync(IEnumerable<Interval> intervals);
        Task<Interval> GetIntervalAsync(Expression<Func<Interval, bool>> expression, bool isEagerLoadingEnabled = false);
        IQueryable<Interval> GetIntervalsCollection(Expression<Func<Interval, bool>> expression, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<Interval>> GetIntervalsCollectionAsync(Expression<Func<Interval, bool>> expression, bool isEagerLoadingEnabled = false);
        bool IsIntervalExist(Expression<Func<Interval, bool>> expression);
        Task<bool> RemoveIntervalAsync(Interval interval);
        Task<bool> UpdateIntervalAsync(Interval interval);
        Task<bool> UpdateIntervalsCollectionAsync(IEnumerable<Interval> intervals);
    }
}