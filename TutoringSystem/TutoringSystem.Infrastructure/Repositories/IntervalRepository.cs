using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class IntervalRepository : RepositoryBase<Interval>, IIntervalRepository
    {
        public IntervalRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddIntervalAsync(Interval interval)
        {
            Create(interval);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddIntervalsCollectionAsync(IEnumerable<Interval> intervals)
        {
            CreateRange(intervals);

            return await SaveChangedAsync();
        }

        public async Task<Interval> GetIntervalAsync(Expression<Func<Interval, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var interval = isEagerLoadingEnabled
                ? await GetIntervalWithEagerLoadingAsync(expression)
                : await GetIntervalWithoutEagerLoadingAsync(expression);

            return interval;
        }

        public IQueryable<Interval> GetIntervalsCollection(Expression<Func<Interval, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var intervals = isEagerLoadingEnabled
                ? GetIntervalsCollectionWithEagerLoading(expression)
                : Find(expression);

            return intervals;
        }

        public bool IsIntervalExist(Expression<Func<Interval, bool>> expression)
        {
            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveIntervalAsync(Interval interval)
        {
            Delete(interval);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateIntervalAsync(Interval interval)
        {
            Update(interval);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateIntervalsCollectionAsync(IEnumerable<Interval> intervals)
        {
            UpdateRange(intervals);

            return await SaveChangedAsync();
        }

        private async Task<Interval> GetIntervalWithEagerLoadingAsync(Expression<Func<Interval, bool>> expression)
        {
            var interval = await Find(expression)
                .Include(i => i.Availability)
                .FirstOrDefaultAsync();

            return interval;
        }

        private async Task<Interval> GetIntervalWithoutEagerLoadingAsync(Expression<Func<Interval, bool>> expression)
        {
            var interval = await Find(expression)
                .FirstOrDefaultAsync();

            return interval;
        }

        private IQueryable<Interval> GetIntervalsCollectionWithEagerLoading(Expression<Func<Interval, bool>> expression)
        {
            var intervals = Find(expression)
                .Include(i => i.Availability);

            return intervals;
        }
    }
}