using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Application.Models.Dtos.Interval;

namespace TutoringSystem.Application.Models.Dtos.Availability
{
    public class UpdatedAvailabilityDto : IMap
    {
        public long Id { get; set; }
        public int BreakTime { get; set; }

        public ICollection<UpdatedIntervalDto> Intervals { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedAvailabilityDto, Domain.Entities.Availability>();
        }
    }
}
