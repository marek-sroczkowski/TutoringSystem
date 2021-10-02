using AutoMapper;
using System;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.IntervalDtos;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AvailabilityDtos
{
    public class AvailabilityDetailsDto : IMap
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<IntervalDto> Intervals { get; set; }
        public TutorDto Tutor { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Availability, AvailabilityDetailsDto>();
        }
    }
}
