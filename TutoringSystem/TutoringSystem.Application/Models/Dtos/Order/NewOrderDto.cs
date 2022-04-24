using AutoMapper;
using System;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Order
{
    public class NewOrderDto : IMap
    {
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public bool IsPaid { get; set; }
        public AdditionalOrderStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewOrderDto, AdditionalOrder>();
        }
    }
}
