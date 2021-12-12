using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class PushNotificationTokenRepository : RepositoryBase<PushNotificationToken>, IPushNotificationTokenRepository
    {
        public PushNotificationTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<PushNotificationToken> GetPushNotificationTokenAsync(Expression<Func<PushNotificationToken, bool>> expression)
        {
            var contact = await DbContext.PushNotificationTokens
                .FirstOrDefaultAsync(expression);

            return contact;
        }

        public async Task<bool> UpdatePushNotificationTokenAsync(PushNotificationToken pushNotificationToken)
        {
            Update(pushNotificationToken);

            return await SaveChangedAsync();
        }
    }
}