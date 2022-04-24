using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Contact
{
    public class UpdatedContactDto : IMap
    {
        public long Id { get; set; }
        public string DiscordName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedContactDto, Domain.Entities.Contact>();
        }
    }
}