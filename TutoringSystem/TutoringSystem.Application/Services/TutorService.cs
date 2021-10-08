using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public TutorService(ITutorRepository tutorRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AddStudentAsync(long tutorId, long studentId)
        {
            var tutor = await tutorRepository.GetTutorByIdAsync(tutorId);
            tutor?.Students?.Add(await studentRepository.GetStudentByIdAsync(studentId));

            return await tutorRepository.UpdateTutorAsync(tutor);
        }

        public async Task<ICollection<StudentDto>> GetStudentsAsync(long tutorId)
        {
            var tutor = await tutorRepository.GetTutorByIdAsync(tutorId);

            return mapper.Map<ICollection<StudentDto>>(tutor?.Students);
        }

        public async Task<TutorDetailsDto> GetTutorAsync(long tutorId)
        {
            var tutor = await tutorRepository.GetTutorByIdAsync(tutorId);

            return mapper.Map<TutorDetailsDto>(tutor);
        }

        public async Task<bool> RemoveStudentAsync(long tutorId, long studentId)
        {
            var tutor = await tutorRepository.GetTutorByIdAsync(tutorId);
            var removed = tutor?.Students?.Remove(await studentRepository.GetStudentByIdAsync(studentId));
            if (!removed.HasValue || !removed.Value)
                return false;

            return await tutorRepository.UpdateTutorAsync(tutor);
        }

        public async Task<bool> RemoveAllStudentsAsync(long tutorId)
        {
            var tutor = await tutorRepository.GetTutorByIdAsync(tutorId);
            tutor?.Students?.Clear();

            return await tutorRepository.UpdateTutorAsync(tutor);
        }
    }
}
