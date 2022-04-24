using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Subject
{
    public class SubjectDetailsDto : IMap
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectPlace Place { get; set; }
        public SubjectCategory Category { get; set; }
        public long TutorId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Subject, SubjectDetailsDto>();
        }
    }
}