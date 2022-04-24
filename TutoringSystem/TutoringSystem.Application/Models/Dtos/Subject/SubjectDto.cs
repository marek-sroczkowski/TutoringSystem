using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Subject
{
    public class SubjectDto : IMap
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public SubjectPlace Place { get; set; }
        public SubjectCategory Category { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Subject, SubjectDto>();
        }

        public SubjectDto()
        {
        }

        public SubjectDto(Domain.Entities.Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            Description = subject.Description;
            Place = subject.Place;
            Category = subject.Category;
        }
    }
}