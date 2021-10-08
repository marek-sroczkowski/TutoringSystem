using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository studentRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IMapper mapper;

        public StudentService(IStudentRepository studentRepository, ITutorRepository tutorRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AddTutorAsync(long studentId, long tutorId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            student?.Tutors?.Add(await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId)));

            return await studentRepository.UpdateStudentAsync(student);
        }

        public async Task<ICollection<TutorDto>> GetTutorsAsync(long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));

            return mapper.Map<ICollection<TutorDto>>(student?.Tutors);
        }

        public async Task<StudentDetailsDto> GetStudentAsync(long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));

            return mapper.Map<StudentDetailsDto>(student);
        }

        public async Task<bool> RemoveTutorAsync(long studentId, long tutorId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            var removed = student?.Tutors?.Remove(await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId)));
            if (!removed.HasValue || !removed.Value)
                return false;

            return await studentRepository.UpdateStudentAsync(student);
        }

        public async Task<bool> RemoveAllTutorsAsync(long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            student?.Tutors?.Clear();

            return await studentRepository.UpdateStudentAsync(student);
        }
    }
}
