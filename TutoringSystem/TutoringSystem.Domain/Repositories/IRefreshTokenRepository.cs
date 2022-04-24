using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddTokenAsync(RefreshToken token);
        Task<bool> AddTokensCollectionAsync(IEnumerable<RefreshToken> tokens);
        Task<RefreshToken> GetTokenAsync(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<RefreshToken> GetTokensCollection(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<RefreshToken>> GetTokensCollectionAsync(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool TokenExists(Expression<Func<RefreshToken, bool>> expression, bool? isActive = true);
        Task<bool> RemoveTokenAsync(RefreshToken token);
        Task<bool> UpdateTokenAsync(RefreshToken token);
        Task<bool> UpdateTokensCollectionAsync(IEnumerable<RefreshToken> tokens);
    }
}