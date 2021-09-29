using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.IntervalDtos
{
    public class UpdatedIntervalDto : IMap
    {
        public long Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedIntervalDto, Interval>();
        }
    }
}
