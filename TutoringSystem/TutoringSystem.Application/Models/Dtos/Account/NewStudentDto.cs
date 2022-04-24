using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Account
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
            profile.CreateMap<NewStudentDto, Domain.Entities.Student>()
                .ForMember(dest => dest.Contact, map => map.MapFrom(src => new Domain.Entities.Contact()))
                .ForMember(dest => dest.Address, map => map.MapFrom(src => new Domain.Entities.Address()))
                .ForMember(dest => dest.PushNotificationToken, map => map.MapFrom(src => new Domain.Entities.PushNotificationToken()))
                .ForPath(dest => dest.Contact.PhoneNumbers, map => map.MapFrom(src => new List<Domain.Entities.PhoneNumber>()));
        }
    }
}