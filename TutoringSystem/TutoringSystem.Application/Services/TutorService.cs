using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IStudentTutorRepository studentTutorRepository;
        private readonly IMapper mapper;

        public TutorService(ITutorRepository tutorRepository, 
            IStudentRepository studentRepository, 
            IStudentTutorRepository studentTutorRepository, 
            IMapper mapper)
        {
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
            this.studentTutorRepository = studentTutorRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AddTutorToStudentAsync(long studentId, long tutorId)
        {
            var studentTutors = await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.StudentId.Equals(studentId));
            var existingTutor = studentTutors.FirstOrDefault(st => st.TutorId.Equals(tutorId));
            if (existingTutor != null)
                return await ActivateTutor(existingTutor);

            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            if (student.StudentTutors is null)
                student.StudentTutors = new List<StudentTutor>();

            var tutorsIds = student.StudentTutors.Select(st => st.TutorId);
            if (tutorsIds.Contains(tutorId))
                return false;

            student.StudentTutors.Add(new StudentTutor(studentId, tutorId, 0, null));

            return await studentRepository.UpdateStudentAsync(student);
        }

        public async Task<ICollection<TutorDto>> GetTutorsByStudentIdAsync(long studentId)
        {
            var tutors = (await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.StudentId.Equals(studentId)))
                .Select(st => st.Tutor);

            return mapper.Map<ICollection<TutorDto>>(tutors);
        }

        public async Task<TutorDetailsDto> GetTutorByIdAsync(long tutorId)
        {
            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId));

            return mapper.Map<TutorDetailsDto>(tutor);
        }

        public async Task<bool> RemoveTutorAsync(long studentId, long tutorId)
        {
            var studentTutor = await studentTutorRepository.GetStudentTutorAsync(st => st.StudentId.Equals(studentId) && st.TutorId.Equals(tutorId));
            studentTutor.IsActive = false;

            return await studentTutorRepository.UpdateStudentTutorAsync(studentTutor);
        }

        public async Task<bool> RemoveAllTutorsAsync(long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            student.StudentTutors.ToList().ForEach(st => st.IsActive = false);

            return await studentRepository.UpdateStudentAsync(student);
        }

        private async Task<bool> ActivateTutor(StudentTutor existingStudent)
        {
            existingStudent.IsActive = true;

            return await studentTutorRepository.UpdateStudentTutorAsync(existingStudent);
        }
    }
}
