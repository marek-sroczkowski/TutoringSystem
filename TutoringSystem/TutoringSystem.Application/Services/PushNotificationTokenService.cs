using System;
using System.Threading.Tasks;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class PushNotificationTokenService : IPushNotificationTokenService
    {
        private readonly IPushNotificationTokenRepository tokenRepository;

        public PushNotificationTokenService(IPushNotificationTokenRepository tokenRepository)
        {
            this.tokenRepository = tokenRepository;
        }

        public async Task<bool> PutTokenAsync(long userId, string tokenContent)
        {
            var token = await tokenRepository.GetPushNotificationTokenAsync(t => t.UserId.Equals(userId));
            if(token is null)
            {
                token = new PushNotificationToken();
                token.UserId = userId;
            }
            token.ModificationDate = DateTime.Now;
            token.Token = tokenContent;

            return await tokenRepository.UpdatePushNotificationTokenAsync(token);
        }
    }
}