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
    public class PushNotificationTokenRepository : RepositoryBase<PushNotificationToken>, IPushNotificationTokenRepository
    {
        public PushNotificationTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddTokenAsync(PushNotificationToken token)
        {
            Create(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddTokensCollection(IEnumerable<PushNotificationToken> tokens)
        {
            CreateRange(tokens);

            return await SaveChangedAsync();
        }

        public async Task<PushNotificationToken> GetTokenAsync(Expression<Func<PushNotificationToken, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var contact = isEagerLoadingEnabled
                ? await GetTokenWithEagerLoadingAsync(expression)
                : await GetTokenWithoutEagerLoadingAsync(expression);

            return contact;
        }

        public IQueryable<PushNotificationToken> GetTokensCollection(Expression<Func<PushNotificationToken, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var contact = isEagerLoadingEnabled
                ? GetTokensCollectionWithEagerLoading(expression)
                : Find(expression);

            return contact;
        }

        public async Task<IEnumerable<PushNotificationToken>> GetTokensCollectionAsync(Expression<Func<PushNotificationToken, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var contact = isEagerLoadingEnabled
                ? GetTokensCollectionWithEagerLoading(expression)
                : Find(expression);

            return await contact.ToListAsync();
        }

        public bool IsTokenExist(Expression<Func<PushNotificationToken, bool>> expression)
        {
            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveTokenAsync(PushNotificationToken token)
        {
            Delete(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateTokenAsync(PushNotificationToken token)
        {
            Update(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateTokensCollectionAsync(IEnumerable<PushNotificationToken> tokens)
        {
            UpdateRange(tokens);

            return await SaveChangedAsync();
        }

        private async Task<PushNotificationToken> GetTokenWithEagerLoadingAsync(Expression<Func<PushNotificationToken, bool>> expression)
        {
            var token = await Find(expression)
                .Include(t => t.User)
                .FirstOrDefaultAsync();

            return token;
        }

        private async Task<PushNotificationToken> GetTokenWithoutEagerLoadingAsync(Expression<Func<PushNotificationToken, bool>> expression)
        {
            var token = await Find(expression)
                .FirstOrDefaultAsync();

            return token;
        }

        private IQueryable<PushNotificationToken> GetTokensCollectionWithEagerLoading(Expression<Func<PushNotificationToken, bool>> expression)
        {
            var tokens = Find(expression)
                .Include(t => t.User);

            return tokens;
        }
    }
}