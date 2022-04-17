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
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddTokenAsync(RefreshToken token)
        {
            Create(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddTokensCollectionAsync(IEnumerable<RefreshToken> tokens)
        {
            CreateRange(tokens);

            return await SaveChangedAsync();
        }

        public async Task<RefreshToken> GetTokenAsync(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public IQueryable<RefreshToken> GetTokensCollection(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public async Task<IEnumerable<RefreshToken>> GetTokensCollectionAsync(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
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

        public bool IsTokenExist(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveTokenAsync(RefreshToken token)
        {
            token.IsActive = false;

            return await UpdateTokenAsync(token);
        }

        public async Task<bool> UpdateTokenAsync(RefreshToken token)
        {
            Update(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateTokensCollectionAsync(IEnumerable<RefreshToken> tokens)
        {
            UpdateRange(tokens);

            return await SaveChangedAsync();
        }

        private async Task<RefreshToken> GetTokenWithEagerLoading(Expression<Func<RefreshToken, bool>> expression)
        {
            var token = await Find(expression)
                .Include(t => t.User)
                .FirstOrDefaultAsync();

            return token;
        }

        private async Task<RefreshToken> GetTokenWithoutEagerLoading(Expression<Func<RefreshToken, bool>> expression)
        {
            var token = await Find(expression)
                .FirstOrDefaultAsync();

            return token;
        }

        private IQueryable<RefreshToken> GetTokensCollectionWithEagerLoading(Expression<Func<RefreshToken, bool>> expression)
        {
            var tokens = Find(expression)
                .Include(t => t.User);

            return tokens;
        }
    }
}