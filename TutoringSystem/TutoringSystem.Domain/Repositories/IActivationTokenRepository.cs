using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IActivationTokenRepository
    {
        Task<bool> AddTokenAsync(ActivationToken token);
        Task<bool> AddTokensCollectionAsync(IEnumerable<ActivationToken> tokens);
        Task<ActivationToken> GetTokenAsync(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<ActivationToken> GetTokensCollection(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<ActivationToken>> GetTokensCollectionAsync(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool TokenExists(Expression<Func<ActivationToken, bool>> expression, bool? isActive = true);
        Task<bool> RemoveActivationTokenAsync(ActivationToken token);
        Task<bool> UpdateActivationTokenAsync(ActivationToken token);
        Task<bool> UpdateTokensCollectionAsync(IEnumerable<ActivationToken> tokens);
    }
}