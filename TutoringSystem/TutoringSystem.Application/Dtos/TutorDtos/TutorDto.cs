using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.TutorDtos
{
    public class TutorDto : IMap
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tutor, TutorDto>();
        }
    }
}
