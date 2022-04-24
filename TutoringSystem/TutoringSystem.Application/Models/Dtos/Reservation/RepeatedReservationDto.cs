using AutoMapper;
using System;
using System.Linq;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Reservation
{
    public class RepeatedReservationDto : IMap
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public ReservationPlace Place { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastAddedDate { get; set; }
        public DateTime NextAddedDate { get; set; }
        public ReservationFrequency Frequency { get; set; }
        public long StudentId { get; set; }
        public string Student { get; set; }
        public long TutorId { get; set; }
        public string Tutor { get; set; }
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public long ExampleReservationId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RepeatedReservation, RepeatedReservationDto>();
        }

        public RepeatedReservationDto()
        {
        }

        public RepeatedReservationDto(RepeatedReservation reservation)
        {
            Id = reservation.Id;
            StartTime = reservation.StartTime;
            Duration = reservation.Duration;
            Place = reservation.Place;
            CreationDate = reservation.CreationDate;
            LastAddedDate = reservation.LastAddedDate;
            NextAddedDate = reservation.NextAddedDate;
            Frequency = reservation.Frequency;
            StudentId = reservation.StudentId;
            TutorId = reservation.TutorId;
            SubjectId = reservation.SubjectId;
            ExampleReservationId = reservation.Reservations.First().Id;
        }
    }
}