using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class NewStudentSingleReservationDto : IMap
    {
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }

        public long SubjectId { get; set; }
        public long TutorId { get; set; }
        public long IntervalId { get; set; }

        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap<NewStudentSingleReservationDto, SingleReservation>();
        }
    }
}
