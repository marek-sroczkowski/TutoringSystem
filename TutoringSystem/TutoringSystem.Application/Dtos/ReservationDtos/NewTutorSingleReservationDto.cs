using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class NewTutorSingleReservationDto : IMap
    {
        public double? Cost { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }

        public long SubjectId { get; set; }
        public long StudentId { get; set; }

        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap<NewTutorSingleReservationDto, SingleReservation>();
        }
    }
}
