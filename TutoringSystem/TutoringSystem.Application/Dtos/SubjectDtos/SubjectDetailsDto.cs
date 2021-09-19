using AutoMapper;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.SubjectDtos
{
    public class SubjectDetailsDto : IMap
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public SubjectCategory Category { get; set; }
        public TutorDto Tutor { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Subject, SubjectDetailsDto>();
        }
    }
}
