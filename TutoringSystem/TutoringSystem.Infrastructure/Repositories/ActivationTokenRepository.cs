using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class ActivationTokenRepository : RepositoryBase<ActivationToken>, IActivationTokenRepository
    {
        public ActivationTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddTokenAsync(ActivationToken token)
        {
            Create(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddTokensCollectionAsync(IEnumerable<ActivationToken> tokens)
        {
            CreateRange(tokens);

            return await SaveChangedAsync();
        }

        public async Task<ActivationToken> GetTokenAsync(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var token = isEagerLoadingEnabled
                ? await GetTokenWithEagerLoading(expression)
                : await GetTokenWithoutEagerLoading(expression);

            return token;
        }

        public IQueryable<ActivationToken> GetTokensCollection(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var tokens = isEagerLoadingEnabled
                ? GetTokensCollectionWithEagerLoading(expression)
                : Find(expression);

            return tokens;
        }

        public async Task<IEnumerable<ActivationToken>> GetTokensCollectionAsync(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            var tokens = isEagerLoadingEnabled
                ? GetTokensCollectionWithEagerLoading(expression)
                : Find(expression);

            return await tokens.ToListAsync();
        }

        public bool IsTokenExist(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveActivationTokenAsync(ActivationToken token)
        {
            token.IsActive = false;

            return await UpdateActivationTokenAsync(token);
        }

        public async Task<bool> UpdateActivationTokenAsync(ActivationToken token)
        {
            Update(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateTokensCollectionAsync(IEnumerable<ActivationToken> tokens)
        {
            UpdateRange(tokens);

            return await SaveChangedAsync();
        }

        private async Task<ActivationToken> GetTokenWithEagerLoading(Expression<Func<ActivationToken, bool>> expression)
        {
            var token = await Find(expression)
                .Include(t => t.User)
                .FirstOrDefaultAsync();

            return token;
        }

        private async Task<ActivationToken> GetTokenWithoutEagerLoading(Expression<Func<ActivationToken, bool>> expression)
        {
            var token = await Find(expression)
                .FirstOrDefaultAsync();

            return token;
        }

        private IQueryable<ActivationToken> GetTokensCollectionWithEagerLoading(Expression<Func<ActivationToken, bool>> expression)
        {
            var tokens = Find(expression)
                .Include(t => t.User);

            return tokens;
        }
    }
}