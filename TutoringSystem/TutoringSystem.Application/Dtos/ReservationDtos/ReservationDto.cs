using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class ReservationDto : IMap
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public ReservationPlace Place { get; set; }
        public ReservationType Type { get; set; }
        public string SubjectName { get; set; }
        public long TutorId { get; set; }
        public string Tutor { get; set; }
        public long StudentId { get; set; }
        public string Student { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Reservation, ReservationDto>()
                .ForMember(dto => dto.SubjectName, map => map.MapFrom(entity => entity.Subject.Name))
                .ForMember(dto => dto.Tutor, map => map.MapFrom(entity => $"{entity.Tutor.FirstName} {entity.Tutor.LastName}"))
                .ForMember(dto => dto.Student, map => map.MapFrom(entity => $"{entity.Student.FirstName} {entity.Student.LastName}"));
        }
    }
}
