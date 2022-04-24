using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Account
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
            profile.CreateMap<RegisteredTutorDto, Domain.Entities.Tutor>()
                .ForMember(dest => dest.Contact, map => map.MapFrom(src => new Domain.Entities.Contact()))
                .ForMember(dest => dest.Address, map => map.MapFrom(src => new Domain.Entities.Address()))
                .ForMember(dest => dest.PushNotificationToken, map => map.MapFrom(src => new Domain.Entities.PushNotificationToken()))
                .ForPath(dest => dest.Contact.Email, map => map.MapFrom(src => src.Email))
                .ForPath(dest => dest.Contact.PhoneNumbers, map => map.MapFrom(src => new List<Domain.Entities.PhoneNumber>()));
        }
    }
}