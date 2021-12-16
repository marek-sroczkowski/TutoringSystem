using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.StudentRequestDtos
{
    public class StudentRequestDto : IMap
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public long StudentId { get; set; }
        public string StudentUsername { get; set; }
        public string StudentName { get; set; }
        public long TutorId { get; set; }
        public string TutorUsername { get; set; }
        public string TutorName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StudentTutorRequest, StudentRequestDto>()
                .ForMember(dto => dto.StudentUsername, map => map.MapFrom(entity => entity.Student.Username))
                .ForMember(dto => dto.StudentName, map => map.MapFrom(entity => $"{entity.Student.FirstName} {entity.Student.LastName}"))
                .ForMember(dto => dto.TutorUsername, map => map.MapFrom(entity => entity.Tutor.Username))
                .ForMember(dto => dto.TutorName, map => map.MapFrom(entity => $"{entity.Tutor.FirstName} {entity.Tutor.LastName}"));
        }
    }
}