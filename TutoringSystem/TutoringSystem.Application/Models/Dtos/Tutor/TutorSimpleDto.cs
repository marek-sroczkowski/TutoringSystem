using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Tutor
{
    public class TutorSimpleDto : IMap
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string TutorName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Tutor, TutorSimpleDto>()
                .ForMember(ts => ts.TutorName, map => map.MapFrom(tutor => $"{tutor.FirstName} {tutor.LastName}"));
        }
    }
}