using AutoMapper;
using System;
using System.Collections.Generic;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Application.Models.Dtos.Interval;

namespace TutoringSystem.Application.Models.Dtos.Availability
{
    public class AvailabilityDto : IMap
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<IntervalDto> Intervals { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Availability, AvailabilityDto>();
        }
    }
}
