using System.Linq;
using TutoringSystem.Application.Dtos.AddressDtos;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.TutorDtos
{
    public class TutorDetailsDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }

        public ContactDto Contact { get; set; }
        public AddressDto Address { get; set; }

        //public IEnumerable<SubjectDto> Subjects { get; set; }

        public TutorDetailsDto()
        {
        }

        public TutorDetailsDto(Tutor tutor)
        {
            Id = tutor.Id;
            Username = tutor.Username;
            FirstName = tutor.FirstName;
            LastName = tutor.LastName;
            Contact = new ContactDto(tutor.Contact);
            Address = new AddressDto(tutor.Address);
            //Subjects = tutor.Subjects.Select(s => new SubjectDto(s));
        }

        public TutorDetailsDto(Tutor tutor, long studentId) : this(tutor)
        {
            var studentTutor = tutor.StudentTutors.FirstOrDefault(st => st.TutorId.Equals(tutor.Id) && st.StudentId.Equals(studentId));
            HourlRate = studentTutor.HourlRate;
        }
    }
}
