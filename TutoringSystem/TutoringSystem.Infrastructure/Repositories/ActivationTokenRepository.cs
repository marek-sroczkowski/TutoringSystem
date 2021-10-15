using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public async Task<bool> AddActivationTokenAsync(ActivationToken token)
        {
            Create(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateActivationTokenAsync(ActivationToken token)
        {
            Update(token);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteActivationTokenAsync(ActivationToken token)
        {
            token.IsActiv = false;

            return await UpdateActivationTokenAsync(token);
        }

        public async Task<ActivationToken> GetActivationTokenAsync(Expression<Func<ActivationToken, bool>> expression)
        {
            var token = await DbContext.ActivationTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(expression);

            return token;
        }

        public async Task<IEnumerable<ActivationToken>> GetActivationTokensCollectionAsync(Expression<Func<ActivationToken, bool>> expression)
        {
            var tokens = await FindByCondition(expression)
                .ToListAsync();

            return tokens;
        }
    }
}
