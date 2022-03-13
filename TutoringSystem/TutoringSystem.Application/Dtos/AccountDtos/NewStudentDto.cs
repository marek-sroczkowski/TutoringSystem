using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class NewStudentDto : IMap
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }
        public string Username { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewStudentDto, Student>()
                .ForMember(dest => dest.Contact, map => map.MapFrom(src => new Contact()))
                .ForMember(dest => dest.Address, map => map.MapFrom(src => new Address()))
                .ForMember(dest => dest.PushNotificationToken, map => map.MapFrom(src => new PushNotificationToken()))
                .ForPath(dest => dest.Contact.PhoneNumbers, map => map.MapFrom(src => new List<PhoneNumber>()));
        }
    }
}