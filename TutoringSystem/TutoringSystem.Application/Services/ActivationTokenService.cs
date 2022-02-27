using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ActivationTokenDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ActivationTokenService : IActivationTokenService
    {
        private readonly IActivationTokenRepository activationTokenRepository;
        private readonly IMapper mapper;

        public ActivationTokenService(IActivationTokenRepository activationTokenRepository, IMapper mapper)
        {
            this.activationTokenRepository = activationTokenRepository;
            this.mapper = mapper;
        }

        public async Task<ActivationTokenDto> AddActivationTokenAsync(long userId)
        {
            await DeactivateTokenAsync(userId);

            var generatedToken = new NewActivationTokenDto(GenerateActivationToken(), userId);
            var token = mapper.Map<ActivationToken>(generatedToken);
            var created = await activationTokenRepository.AddTokenAsync(token);

            return created ? mapper.Map<ActivationTokenDto>(token) : null;
        }

        private async Task DeactivateTokenAsync(long userId)
        {
            var token = await activationTokenRepository.GetTokenAsync(t => t.UserId.Equals(userId) && t.ExpirationDate >= DateTime.Now);
            if (token is null)
            {
                return;
            }

            token.ExpirationDate = DateTime.Now;
            await activationTokenRepository.UpdateActivationTokenAsync(token);
        }

        private static string GenerateActivationToken()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
