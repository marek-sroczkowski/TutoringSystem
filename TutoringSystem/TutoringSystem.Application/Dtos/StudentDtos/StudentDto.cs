using System.Linq;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.StudentDtos
{
    public class StudentDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double HourlRate { get; set; }
        public string Note { get; set; }

        public StudentDto()
        {
        }

        public StudentDto(Student student)
        {
            Id = student.Id;
            Username = student.Username;
            FirstName = student.FirstName;
            LastName = student.LastName;
        }

        public StudentDto(Student student, long tutorId) : this(student)
        {
            var studentTutor = student.StudentTutors.FirstOrDefault(st => st.StudentId.Equals(student.Id) && st.TutorId.Equals(tutorId));
            HourlRate = studentTutor.HourlRate;
            Note = studentTutor.Note;
        }
    }
}
