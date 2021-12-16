using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.StudentDtos
{
    public class StudentSimpleDto : IMap
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string StudentName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Student, StudentSimpleDto>()
                .ForMember(ts => ts.StudentName, map => map.MapFrom(student => $"{student.FirstName} {student.LastName}"));
        }
    }
}