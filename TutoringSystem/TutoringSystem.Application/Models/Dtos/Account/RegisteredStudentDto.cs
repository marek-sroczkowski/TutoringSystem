using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Account
{
    public class RegisteredStudentDto : IMap
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisteredStudentDto, Domain.Entities.Student>();
        }
    }
}