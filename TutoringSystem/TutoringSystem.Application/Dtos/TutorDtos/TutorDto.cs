using System.Linq;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.TutorDtos
{
    public class TutorDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string ProfilePictureBase64 { get; set; }

        public TutorDto()
        {
        }

        public TutorDto(Tutor tutor)
        {
            Id = tutor.Id;
            Username = tutor.Username;
            FirstName = tutor.FirstName;
            LastName = tutor.LastName;
            ProfilePictureBase64 = tutor.ProfilePictureBase64;
        }

        public TutorDto(Tutor tutor, long studentId) : this(tutor)
        {
            var studentTutor = tutor.StudentTutors.FirstOrDefault(st => st.TutorId.Equals(tutor.Id) && st.StudentId.Equals(studentId));
            HourlRate = studentTutor.HourlRate;
        }
    }
}