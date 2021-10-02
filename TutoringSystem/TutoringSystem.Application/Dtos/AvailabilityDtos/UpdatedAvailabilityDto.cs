using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.IntervalDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AvailabilityDtos
{
    public class UpdatedAvailabilityDto : IMap
    {
        public long Id { get; set; }
        public int BreakTime { get; set; }

        public ICollection<UpdatedIntervalDto> Intervals { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedAvailabilityDto, Availability>();
        }
    }
}
