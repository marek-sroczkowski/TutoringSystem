using System.Linq;
using TutoringSystem.Application.Models.Dtos.Address;
using TutoringSystem.Application.Models.Dtos.Contact;

namespace TutoringSystem.Application.Models.Dtos.Tutor
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

        public TutorDetailsDto(Domain.Entities.Tutor tutor)
        {
            Id = tutor.Id;
            Username = tutor.Username;
            FirstName = tutor.FirstName;
            LastName = tutor.LastName;
            Contact = new ContactDto(tutor.Contact);
            Address = new AddressDto(tutor.Address);
            //Subjects = tutor.Subjects.Select(s => new SubjectDto(s));
        }

        public TutorDetailsDto(Domain.Entities.Tutor tutor, long studentId) : this(tutor)
        {
            var studentTutor = tutor.StudentTutors.FirstOrDefault(st => st.TutorId.Equals(tutor.Id) && st.StudentId.Equals(studentId));
            HourlRate = studentTutor.HourlRate;
        }
    }
}
