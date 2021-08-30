using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.SchoolDtos
{
    public class SchoolDto : IMap
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public int Year { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<School, SchoolDto>();
        }
    }
}
