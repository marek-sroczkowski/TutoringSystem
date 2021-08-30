using AutoMapper;
using System.Collections.Generic;
using TutoringSystem.Application.Dtos.AddressDtos;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.TutorDtos
{
    public class TutorDetailsDto : IMap
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ContactDto Contact { get; set; }
        public AddressDto Address { get; set; }

        public ICollection<SubjectDto> Subjects { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tutor, TutorDetailsDto>();
        }
    }
}
