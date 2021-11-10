using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.TutorDtos
{
    public class TutorDto : IMap
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tutor, TutorDto>();
        }

        public TutorDto()
        {
        }

        public TutorDto(Tutor tutor)
        {
            Id = tutor.Id;
            Username = tutor.Username;
            FirstName = tutor.FirstName;
            LastName = tutor.LastName;
        }
    }
}
