using System.Linq;
using TutoringSystem.Application.Dtos.AddressDtos;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.StudentDtos
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

        public StudentDetailsDto(Student student)
        {
            Id = student.Id;
            Username = student.Username;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Contact = new ContactDto(student.Contact);
            Address = new AddressDto(student.Address);
        }

        public StudentDetailsDto(Student student, long tutorId) : this(student)
        {
            var studentTutor = student.StudentTutors.FirstOrDefault(st => st.StudentId.Equals(student.Id) && st.TutorId.Equals(tutorId));
            HourlRate = studentTutor.HourlRate;
            Note = studentTutor.Note;
        }
    }
}
