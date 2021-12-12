using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IPushNotificationTokenRepository
    {
        Task<PushNotificationToken> GetPushNotificationTokenAsync(Expression<Func<PushNotificationToken, bool>> expression);
        Task<bool> UpdatePushNotificationTokenAsync(PushNotificationToken pushNotificationToken);
    }
}
