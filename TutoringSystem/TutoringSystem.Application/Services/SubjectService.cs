using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository subjectRepository;
        private readonly IMapper mapper;

        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            this.subjectRepository = subjectRepository;
            this.mapper = mapper;
        }

        public async Task<bool> ActivateSubjectAsync(long tutorId, long subjectId)
        {
            var inactiveSubject = await subjectRepository.GetSubjectsByTutorAsync(tutorId, false);
            var subject = inactiveSubject?.FirstOrDefault(s => s.Id.Equals(subjectId));
            if (subject is null)
                return false;
            subject.IsActiv = true;

            return await subjectRepository.UpdateSubjectAsync(subject);
        }

        public async Task<SubjectDto> AddSubjectAsync(long tutorId, NewSubjectDto newSubjectModel)
        {
            var subject = mapper.Map<Subject>(newSubjectModel);
            subject.TutorId = tutorId;

            var created = await subjectRepository.AddSubjectAsync(subject);
            if (!created)
                return null;

            return mapper.Map<SubjectDto>(subject);
        }

        public async Task<bool> DeactivateSubjectAsync(long subjectId)
        {
            var subject = await subjectRepository.GetSubjectByIdAsync(subjectId);

            return await subjectRepository.DeleteSubjectAsync(subject);
        }

        public async Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId)
        {
            var subject = await subjectRepository.GetSubjectByIdAsync(subjectId);

            return mapper.Map<SubjectDetailsDto>(subject);
        }

        public async Task<ICollection<SubjectDto>> GetTutorSubjectsAsync(long tutorId, bool isActiv = true)
        {
            var subjects = await subjectRepository.GetSubjectsByTutorAsync(tutorId, isActiv);

            return mapper.Map<ICollection<SubjectDto>>(subjects);
        }

        public async Task<bool> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject)
        {
            var existingSubject = await subjectRepository.GetSubjectByIdAsync(updatedSubject.Id);
            var subject = mapper.Map(updatedSubject, existingSubject);

            return await subjectRepository.UpdateSubjectAsync(subject);
        }
    }
}
