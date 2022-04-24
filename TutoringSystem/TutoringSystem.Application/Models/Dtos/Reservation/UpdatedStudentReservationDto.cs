using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Reservation
{
    public class UpdatedStudentReservationDto : IMap
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }

        public long SubjectId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedStudentReservationDto, Domain.Entities.Reservation>();
        }
    }
}
