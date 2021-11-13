using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Enums;
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

        public StudentService(IStudentRepository studentRepository,
            ITutorRepository tutorRepository,
            IStudentTutorRepository studentTutorRepository)
        {
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
            this.studentTutorRepository = studentTutorRepository;
        }

        public async Task<AddStudentToTutorStatus> AddStudentToTutorAsync(long tutorId, NewExistingStudentDto newStudent)
        {
            var studentTutors = await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.TutorId.Equals(tutorId), null);
            var existingStudentTutor = studentTutors.FirstOrDefault(st => st.Student.Username.Equals(newStudent.Username));
            if (existingStudentTutor != null && existingStudentTutor.IsActive == false)
                return await ActivateStudent(newStudent, existingStudentTutor) ? AddStudentToTutorStatus.Added : AddStudentToTutorStatus.InternalError;
            else if (existingStudentTutor != null && existingStudentTutor.IsActive == true)
                return AddStudentToTutorStatus.StudentWasAlreadyAdded;

            var student = await studentRepository.GetStudentAsync(s => s.Username.Equals(newStudent.Username));
            if (student is null)
                return AddStudentToTutorStatus.IncorrectUsername;

            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId));
            if (tutor.StudentTutors is null)
                tutor.StudentTutors = new List<StudentTutor>();

            var studentTutor = new StudentTutor(student.Id, tutorId, newStudent.HourRate, newStudent.Note);
            tutor.StudentTutors.Add(studentTutor);

            return await tutorRepository.UpdateTutorAsync(tutor) ? AddStudentToTutorStatus.Added : AddStudentToTutorStatus.InternalError;
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

        public async Task<bool> UpdateStudentAsync(long tutorId, UpdatedStudentDto student)
        {
            var studentTutor = await studentTutorRepository.GetStudentTutorAsync(st => st.StudentId.Equals(student.StudentId) && st.TutorId.Equals(tutorId));
            if (studentTutor is null)
                return false;

            studentTutor.HourlRate = student.HourRate;
            studentTutor.Note = student.Note;

            return await studentTutorRepository.UpdateStudentTutorAsync(studentTutor);
        }

        private async Task<bool> ActivateStudent(NewExistingStudentDto student, StudentTutor existingStudent)
        {
            return await studentTutorRepository
                .UpdateStudentTutorAsync(new StudentTutor(existingStudent.StudentId, existingStudent.TutorId, student.HourRate, student.Note));
        }
    }
}
