﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Interval> GetIntervalByIdAsync(long intervalId)
        {
            var interval = await DbContext.Intervals
                .Include(i => i.Availability)
                .FirstOrDefaultAsync(i => i.Id.Equals(intervalId));

            return interval;
        }
    }
}