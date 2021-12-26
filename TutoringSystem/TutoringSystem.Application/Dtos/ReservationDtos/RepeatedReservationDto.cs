using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class RepeatedReservationDto : IMap
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }
        public DateTime NextAddedDate { get; set; }
        public ReservationFrequency Frequency { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RepeatedReservation, RepeatedReservationDto>();
        }
    }
}