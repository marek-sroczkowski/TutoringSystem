using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
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
        private readonly IStudentTutorRequestRepository requestRepository;
        private readonly IMapper mapper;

        public StudentService(IStudentRepository studentRepository,
            ITutorRepository tutorRepository,
            IStudentTutorRepository studentTutorRepository,
            IStudentTutorRequestRepository requestRepository,
            IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
            this.studentTutorRepository = studentTutorRepository;
            this.requestRepository = requestRepository;
            this.mapper = mapper;
        }

        public async Task<AddStudentToTutorStatus> AddStudentToTutorAsync(long tutorId, NewExistingStudentDto newStudent)
        {
            var request = await requestRepository.GetRequestAsync(r => r.StudentId.Equals(newStudent.StudentId) && r.TutorId.Equals(tutorId));
            if (request != null && request.IsActive)
            {
                request.IsAccepted = true;
                request.IsActive = false;
                if (!(await requestRepository.UpdateRequestAsync(request)))
                    return AddStudentToTutorStatus.InternalError;
            }

            var existingStudentTutor = await studentTutorRepository.GetStudentTutorAsync(st => st.StudentId.Equals(newStudent.StudentId) && st.TutorId.Equals(tutorId), null);
            if (existingStudentTutor != null && !existingStudentTutor.IsActive)
                return await ActivateStudent(newStudent, existingStudentTutor);
            else if (existingStudentTutor != null && existingStudentTutor.IsActive)
                return AddStudentToTutorStatus.StudentWasAlreadyAdded;

            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(newStudent.StudentId));
            if (student is null)
                return AddStudentToTutorStatus.StudentNotExist;

            var studentTutor = new StudentTutor(student.Id, tutorId, newStudent.HourRate, newStudent.Note);

            return await studentTutorRepository.AddStudentTutorAsync(studentTutor) ?
                AddStudentToTutorStatus.Added : AddStudentToTutorStatus.InternalError;
        }

        public async Task<IEnumerable<StudentDto>> GetStudentsByTutorIdAsync(long tutorId)
        {
            var students = (await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.TutorId.Equals(tutorId)))
                .Select(st => st.Student);

            return students.Select(s => new StudentDto(s, tutorId)).ToList();
        }

        public async Task<PagedList<StudentSimpleDto>> GetStudents(SearchedUserParameters parameters)
        {
            var students = string.IsNullOrWhiteSpace(parameters.Params) ?
                new List<Student>() :
                await studentRepository.GetStudentsCollectionAsync(GetExpressionToSearchedStudents(parameters));
            var studentDtos = mapper.Map<ICollection<StudentSimpleDto>>(students);

            return PagedList<StudentSimpleDto>.ToPagedList(studentDtos, parameters.PageNumber, parameters.PageSize);
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
            studentTutor.Student.FirstName = student.FirstName;
            studentTutor.Student.LastName = student.LastName;

            return await studentTutorRepository.UpdateStudentTutorAsync(studentTutor);
        }

        private Expression<Func<Student, bool>> GetExpressionToSearchedStudents(SearchedUserParameters parameters)
        {
            Expression<Func<Student, bool>> expression = r => r.Username.ToLower().Contains(parameters.Params.Trim().ToLower()) ||
                r.FirstName.ToLower().Contains(parameters.Params.Trim().ToLower()) ||
                r.LastName.ToLower().Contains(parameters.Params.Trim().ToLower());

            return expression;
        }

        private async Task<AddStudentToTutorStatus> ActivateStudent(NewExistingStudentDto student, StudentTutor existingStudent)
        {
            existingStudent.HourlRate = student.HourRate;
            existingStudent.Note = student.Note;
            existingStudent.IsActive = true;

            return await studentTutorRepository
                .UpdateStudentTutorAsync(existingStudent) ?
                AddStudentToTutorStatus.Added : AddStudentToTutorStatus.InternalError;
        }
    }
}
