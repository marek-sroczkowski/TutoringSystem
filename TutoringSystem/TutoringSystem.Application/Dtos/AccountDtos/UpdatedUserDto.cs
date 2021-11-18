using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class UpdatedUserDto : IMap
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatedUserDto, User>();
        }
    }
}
