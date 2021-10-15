using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IActivationTokenRepository
    {
        Task<bool> AddActivationTokenAsync(ActivationToken token);
        Task<bool> DeleteActivationTokenAsync(ActivationToken token);
        Task<ActivationToken> GetActivationTokenAsync(Expression<Func<ActivationToken, bool>> expression);
        Task<IEnumerable<ActivationToken>> GetActivationTokensCollectionAsync(Expression<Func<ActivationToken, bool>> expression);
        Task<bool> UpdateActivationTokenAsync(ActivationToken token);
    }
}
