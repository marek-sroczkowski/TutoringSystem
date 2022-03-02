using System;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
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
            var token = await tokenRepository.GetTokenAsync(t => t.UserId.Equals(userId));
            if (token is null)
            {
                token = new PushNotificationToken
                {
                    UserId = userId
                };
            }

            token.ModificationDate = DateTime.Now.ToLocal();
            token.Token = tokenContent;

            return await tokenRepository.UpdateTokenAsync(token);
        }
    }
}