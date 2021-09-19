using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.StudentDtos
{
    public class StudentDto : IMap
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Student, StudentDto>();
        }
    }
}
