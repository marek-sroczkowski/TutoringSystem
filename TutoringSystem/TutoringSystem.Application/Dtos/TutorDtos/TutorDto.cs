using AutoMapper;
using System.Linq;
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
        public double HourlRate { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }

        public TutorDto()
        {
        }

        public TutorDto(Tutor tutor)
        {
            Id = tutor.Id;
            Username = tutor.Username;
            FirstName = tutor.FirstName;
            LastName = tutor.LastName;
            ProfilePictureFirebaseUrl = tutor.ProfilePictureFirebaseUrl;
        }

        public TutorDto(Tutor tutor, long studentId) : this(tutor)
        {
            var studentTutor = tutor.StudentTutors.FirstOrDefault(st => st.TutorId.Equals(tutor.Id) && st.StudentId.Equals(studentId));
            HourlRate = studentTutor.HourlRate;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tutor, TutorDto>();
        }
    }
}