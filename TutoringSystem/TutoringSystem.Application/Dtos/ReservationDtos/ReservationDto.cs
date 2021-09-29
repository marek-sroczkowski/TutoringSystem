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
        public double Duration { get; set; }
        public string Description { get; set; }
        public ReservationPlace Place { get; set; }
        public string SubjectName { get; set; }
        public string Tutor { get; set; }
        public string Student { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Reservation, ReservationDto>()
                .ForMember(dto => dto.SubjectName, map => map.MapFrom(reservation => reservation.Subject.Name))
                .ForMember(dto => dto.Tutor, map => map.MapFrom(reservation => (reservation.Tutor.FirstName + " " + reservation.Tutor.LastName)))
                .ForMember(dto => dto.Student, map => map.MapFrom(reservation => (reservation.Student.FirstName + " " + reservation.Student.LastName)));
        }
    }
}
