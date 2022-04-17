using Microsoft.EntityFrameworkCore;
using System.Linq;
using TutoringSystem.Domain.Entities.Base;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Extensions
{
    public static class DbContextExtension
    {
        public static void DetachLocal<T>(this AppDbContext dbContext, T t, long entryId) where T : Entity
        {
            var local = dbContext.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));

            if (local != null)
            {
                dbContext.Entry(local).State = EntityState.Detached;
            }

            dbContext.Entry(t).State = EntityState.Modified;
        }
    }
}