using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Interval
{
    public class IntervalDto : IMap
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Interval, IntervalDto>();
        }
    }
}
