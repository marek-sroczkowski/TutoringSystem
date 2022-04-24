using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Models.Dtos.Authentication
{
    public class AuthenticationDto : IMap
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceIdentificator { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuthenticationDto, User>();
        }
    }
}