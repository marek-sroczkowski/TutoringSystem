using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentTutorRepository studentTutorRepository;
        private readonly IMapper mapper;

        public StudentService(IStudentRepository studentRepository,
            ITutorRepository tutorRepository,
            IStudentTutorRepository studentTutorRepository, 
            IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
            this.studentTutorRepository = studentTutorRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AddStudentToTutorAsync(long tutorId, NewTutorsStudentDto student)
        {
            var studentTutors = await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.TutorId.Equals(tutorId), null);
            var existingStudent = studentTutors.FirstOrDefault(st => st.StudentId.Equals(student.StudentId));
            if (existingStudent != null)
                return await ActivateStudent(student, existingStudent);

            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId));
            if (tutor.StudentTutors is null)
                tutor.StudentTutors = new List<StudentTutor>();

            var studentsIds = studentTutors.Select(s => s.StudentId);
            if (studentsIds.Contains(student.StudentId))
                return false;

            var studentTutor = mapper.Map<StudentTutor>(student);
            studentTutor.TutorId = tutorId;
            tutor.StudentTutors.Add(studentTutor);

            return await tutorRepository.UpdateTutorAsync(tutor);
        }

        public async Task<ICollection<StudentDto>> GetStudentsByTutorIdAsync(long tutorId)
        {
            var students = (await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.TutorId.Equals(tutorId)))
                .Select(st => st.Student);

            return students.Select(s => new StudentDto(s, tutorId)).ToList();
        }

        public async Task<StudentDetailsDto> GetStudentAsync(long tutorId, long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));

            return new StudentDetailsDto(student, tutorId);
        }

        public async Task<bool> RemoveStudentAsync(long tutorId, long studentId)
        {
            var studentTutor = await studentTutorRepository.GetStudentTutorAsync(st => st.StudentId.Equals(studentId) && st.TutorId.Equals(tutorId));
            studentTutor.IsActive = false;

            return await studentTutorRepository.UpdateStudentTutorAsync(studentTutor);
        }

        public async Task<bool> RemoveAllStudentsAsync(long tutorId)
        {
            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId));
            tutor.StudentTutors.ToList().ForEach(st => st.IsActive = false);

            return await tutorRepository.UpdateTutorAsync(tutor);
        }

        private async Task<bool> ActivateStudent(NewTutorsStudentDto student, StudentTutor existingStudent)
        {
            existingStudent.IsActive = true;
            existingStudent.HourlRate = student.HourlRate;
            existingStudent.Note = student.Note;

            return await studentTutorRepository.UpdateStudentTutorAsync(existingStudent);
        }
    }
}
