using System.Threading.Tasks;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IPushNotificationTokenService
    {
        Task<bool> PutTokenAsync(long userId, string tokenContent);
    }
}