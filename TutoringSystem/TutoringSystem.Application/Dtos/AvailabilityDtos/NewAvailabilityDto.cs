using AutoMapper;
using System;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.IntervalDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AvailabilityDtos
{
    public class NewAvailabilityDto : IMap
    {
        public DateTime Date { get; set; }

        public ICollection<NewIntervalDto> Intervals { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewAvailabilityDto, Availability>();
        }
    }
}
