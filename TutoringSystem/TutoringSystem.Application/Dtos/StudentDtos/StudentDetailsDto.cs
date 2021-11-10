using System.Collections.Generic;
using System.Linq;
using TutoringSystem.Application.Dtos.TutorDtos;
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
        public IEnumerable<TutorDto> Tutors { get; set; }


        public StudentDetailsDto()
        {
        }

        public StudentDetailsDto(Student student)
        {
            Id = student.Id;
            Username = student.Username;
            FirstName = student.FirstName;
            LastName = student.LastName;

            Tutors = student.StudentTutors?.Select(st => new TutorDto(st.Tutor));
        }

        public StudentDetailsDto(Student student, long tutorId) : this(student)
        {
            var studentTutor = student.StudentTutors.FirstOrDefault(st => st.StudentId.Equals(student.Id) && st.TutorId.Equals(tutorId));
            HourlRate = studentTutor.HourlRate;
            Note = studentTutor.Note;
        }
    }
}
