using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class RegisterTutorDto : IMap
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterTutorDto, Tutor>()
                .ForPath(user => user.Contact.Email, map => map.MapFrom(dto => dto.Email));
        }
    }
}
