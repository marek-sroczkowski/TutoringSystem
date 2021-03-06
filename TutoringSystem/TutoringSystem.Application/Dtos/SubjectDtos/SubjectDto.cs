using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.SubjectDtos
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
            profile.CreateMap<Subject, SubjectDto>();
        }

        public SubjectDto()
        {
        }

        public SubjectDto(Subject subject)
        {
            Id = subject.Id;
            Name = subject.Name;
            Description = subject.Description;
            Place = subject.Place;
            Category = subject.Category;
        }
    }
}