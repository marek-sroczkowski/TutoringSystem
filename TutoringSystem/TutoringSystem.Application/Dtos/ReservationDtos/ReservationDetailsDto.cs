using AutoMapper;
using System;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class ReservationDetailsDto : IMap
    {
        public long Id { get; set; }
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }

        public SubjectDto Subject { get; set; }
        public TutorDto Tutor { get; set; }
        public StudentDto Student { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Reservation, ReservationDetailsDto>();
        }
    }
}
