using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.ContactDtos
{
    public class UpdatedContactDto : IMap
    {
        public long Id { get; set; }
        public string DiscordName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedContactDto, Contact>();
        }
    }
}