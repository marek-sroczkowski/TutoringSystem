using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.ActivationTokenDtos
{
    public class NewActivationTokenDto : IMap
    {
        public string TokenContent { get; set; }
        public long UserId { get; set; }

        public NewActivationTokenDto()
        {
        }

        public NewActivationTokenDto(string tokenContent, long userId)
        {
            TokenContent = tokenContent;
            UserId = userId;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewActivationTokenDto, ActivationToken>();
        }
    }
}
