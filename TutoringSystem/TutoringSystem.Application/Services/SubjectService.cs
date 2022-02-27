using AutoMapper;
using System.Collections.Generic;
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

        public async Task<SubjectDto> AddSubjectAsync(long tutorId, NewSubjectDto newSubjectModel)
        {
            var subject = mapper.Map<Subject>(newSubjectModel);
            subject.TutorId = tutorId;
            var created = await subjectRepository.AddSubjectAsync(subject);

            return created ? mapper.Map<SubjectDto>(subject) : null;
        }

        public async Task<bool> DeactivateSubjectAsync(long subjectId)
        {
            var subject = await subjectRepository.GetSubjectAsync(s => s.Id.Equals(subjectId));

            return await subjectRepository.RemoveSubjectAsync(subject);
        }

        public async Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId)
        {
            var subject = await subjectRepository.GetSubjectAsync(s => s.Id.Equals(subjectId));

            return mapper.Map<SubjectDetailsDto>(subject);
        }

        public async Task<IEnumerable<SubjectDto>> GetTutorSubjectsAsync(long tutorId)
        {
            var subjects = await subjectRepository.GetSubjectsCollectionAsync(s => s.TutorId.Equals(tutorId));

            return mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }

        public async Task<bool> UpdateSubjectAsync(UpdatedSubjectDto updatedSubject)
        {
            var existingSubject = await subjectRepository.GetSubjectAsync(s => s.Id.Equals(updatedSubject.Id));
            var subject = mapper.Map(updatedSubject, existingSubject);

            return await subjectRepository.UpdateSubjectAsync(subject);
        }
    }
}