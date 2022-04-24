using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Student
{
    public class StudentSimpleDto : IMap
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string StudentName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Student, StudentSimpleDto>()
                .ForMember(ts => ts.StudentName, map => map.MapFrom(student => $"{student.FirstName} {student.LastName}"));
        }
    }
}