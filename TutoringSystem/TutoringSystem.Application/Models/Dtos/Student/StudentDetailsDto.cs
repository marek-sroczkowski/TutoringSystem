using System.Linq;
using TutoringSystem.Application.Models.Dtos.Address;
using TutoringSystem.Application.Models.Dtos.Contact;

namespace TutoringSystem.Application.Models.Dtos.Student
{
    public class StudentDetailsDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }
        public ContactDto Contact { get; set; }
        public AddressDto Address { get; set; }


        public StudentDetailsDto()
        {
        }

        public StudentDetailsDto(Domain.Entities.Student student)
        {
            Id = student.Id;
            Username = student.Username;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Contact = new ContactDto(student.Contact);
            Address = new AddressDto(student.Address);
        }

        public StudentDetailsDto(Domain.Entities.Student student, long tutorId) : this(student)
        {
            var studentTutor = student.StudentTutors.FirstOrDefault(st => st.StudentId.Equals(student.Id) && st.TutorId.Equals(tutorId));
            HourlRate = studentTutor.HourlRate;
            Note = studentTutor.Note;
        }
    }
}
