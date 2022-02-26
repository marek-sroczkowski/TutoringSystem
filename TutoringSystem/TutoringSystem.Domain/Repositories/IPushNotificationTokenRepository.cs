using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IPushNotificationTokenRepository
    {
        Task<bool> AddTokenAsync(PushNotificationToken token);
        Task<bool> AddTokensCollection(IEnumerable<PushNotificationToken> tokens);
        Task<PushNotificationToken> GetTokenAsync(Expression<Func<PushNotificationToken, bool>> expression, bool isEagerLoadingEnabled = false);
        IQueryable<PushNotificationToken> GetTokensCollection(Expression<Func<PushNotificationToken, bool>> expression, bool isEagerLoadingEnabled = false);
        bool IsTokenExist(Expression<Func<PushNotificationToken, bool>> expression);
        Task<bool> RemoveTokenAsync(PushNotificationToken token);
        Task<bool> UpdateTokenAsync(PushNotificationToken pushNotificationToken);
        Task<bool> UpdateTokensCollectionAsync(IEnumerable<PushNotificationToken> tokens);
    }
}