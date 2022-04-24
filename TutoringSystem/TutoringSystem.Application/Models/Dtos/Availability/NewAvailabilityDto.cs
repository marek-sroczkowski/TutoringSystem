using AutoMapper;
using System;
using System.Collections.Generic;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Application.Models.Dtos.Interval;

namespace TutoringSystem.Application.Models.Dtos.Availability
{
    public class NewAvailabilityDto : IMap
    {
        public DateTime Date { get; set; }
        public int BreakTime { get; set; }

        public ICollection<NewIntervalDto> Intervals { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewAvailabilityDto, Domain.Entities.Availability>();
        }
    }
}
