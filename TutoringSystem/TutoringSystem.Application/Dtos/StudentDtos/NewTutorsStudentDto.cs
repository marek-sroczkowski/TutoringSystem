using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.StudentDtos
{
    public class NewTutorsStudentDto : IMap
    {
        public long StudentId { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewTutorsStudentDto, StudentTutor>();
        }
    }
}
