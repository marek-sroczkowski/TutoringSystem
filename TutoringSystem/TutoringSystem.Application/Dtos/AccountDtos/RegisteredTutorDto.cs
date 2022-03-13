using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class RegisteredTutorDto : IMap
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisteredTutorDto, Tutor>()
                .ForMember(dest => dest.Contact, map => map.MapFrom(src => new Contact()))
                .ForMember(dest => dest.Address, map => map.MapFrom(src => new Address()))
                .ForMember(dest => dest.PushNotificationToken, map => map.MapFrom(src => new PushNotificationToken()))
                .ForPath(dest => dest.Contact.Email, map => map.MapFrom(src => src.Email))
                .ForPath(dest => dest.Contact.PhoneNumbers, map => map.MapFrom(src => new List<PhoneNumber>()));
        }
    }
}