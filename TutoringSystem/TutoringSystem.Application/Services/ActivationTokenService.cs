using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Token;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ActivationTokenService : IActivationTokenService
    {
        private readonly IActivationTokenRepository activationTokenRepository;
        private readonly AppSettings settings;
        private readonly IMapper mapper;

        public ActivationTokenService(IActivationTokenRepository activationTokenRepository, IOptions<AppSettings> settings, IMapper mapper)
        {
            this.activationTokenRepository = activationTokenRepository;
            this.settings = settings.Value;
            this.mapper = mapper;
        }

        public async Task<ActivationTokenDetailsDto> AddActivationTokenAsync(long userId)
        {
            await DeactivateTokenAsync(userId);

            var generatedToken = GenerateActivationToken(userId);
            var token = mapper.Map<ActivationToken>(generatedToken);
            var created = await activationTokenRepository.AddTokenAsync(token);

            return created ? mapper.Map<ActivationTokenDetailsDto>(token) : null;
        }

        private async Task DeactivateTokenAsync(long userId)
        {
            var now = DateTime.Now.ToLocal();
            var token = await activationTokenRepository.GetTokenAsync(t => t.UserId.Equals(userId) && t.ExpirationDate >= now);
            if (token is null)
            {
                return;
            }

            token.ExpirationDate = DateTime.Now.ToLocal();
            await activationTokenRepository.UpdateActivationTokenAsync(token);
        }

        private NewActivationTokenDto GenerateActivationToken(long userId)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var content = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            return new NewActivationTokenDto
            {
                TokenContent = content,
                UserId = userId,
                ExpirationDate = DateTime.Now.ToLocal().AddDays(settings.ActivationTokenExpireDays)
            };
        }
    }
}