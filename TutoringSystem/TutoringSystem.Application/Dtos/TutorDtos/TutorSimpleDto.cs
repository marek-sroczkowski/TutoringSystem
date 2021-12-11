using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.TutorDtos
{
    public class TutorSimpleDto : IMap
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string TutorName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tutor, TutorSimpleDto>()
                .ForMember(ts => ts.TutorName, map => map.MapFrom(tutor => $"{tutor.FirstName} {tutor.LastName}"));
        }
    }
}