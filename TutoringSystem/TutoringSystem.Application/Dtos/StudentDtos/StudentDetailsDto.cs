using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.StudentDtos
{
    public class StudentDetailsDto : IMap
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public ICollection<TutorDto> Tutors { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Student, StudentDetailsDto>();
        }
    }
}
