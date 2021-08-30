﻿using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class CreateStudentReservationDto : IMap
    {
        public double Cost { get; set; }
        public DateTime StartTime { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public Place Place { get; set; }

        public long SubjectId { get; set; }
        public long TutorId { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateStudentReservationDto, Reservation>();
        }
    }
}
