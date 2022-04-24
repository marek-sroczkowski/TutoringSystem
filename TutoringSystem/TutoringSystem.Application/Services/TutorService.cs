using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Models.Dtos.Tutor;
using TutoringSystem.Application.Models.Parameters;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentTutorRepository studentTutorRepository;
        private readonly IStudentTutorRequestRepository requestRepository;
        private readonly IMapper mapper;

        public TutorService(ITutorRepository tutorRepository,
            IStudentTutorRepository studentTutorRepository,
            IStudentTutorRequestRepository requestRepository,
            IMapper mapper)
        {
            this.tutorRepository = tutorRepository;
            this.studentTutorRepository = studentTutorRepository;
            this.requestRepository = requestRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TutorDto>> GetTutorsByStudentIdAsync(long studentId)
        {
            var tutors = (await studentTutorRepository.GetStudentTutorCollectionAsync(st => st.StudentId.Equals(studentId), isEagerLoadingEnabled: true))
                .Select(st => st.Tutor);

            return tutors.Select(t => new TutorDto(t, studentId));
        }

        public async Task<PagedList<TutorSimpleDto>> GetTutors(SearchedUserParameters parameters)
        {
            var tutors = string.IsNullOrWhiteSpace(parameters.Params) ?
                new List<Tutor>() :
                await tutorRepository.GetTutorsCollectionAsync(GetExpressionToSearchedTutors(parameters));

            var tutorDtos = mapper.Map<IEnumerable<TutorSimpleDto>>(tutors);

            return PagedList<TutorSimpleDto>.ToPagedList(tutorDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<TutorDetailsDto> GetTutorAsync(long tutorId, long studentId)
        {
            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId), isEagerLoadingEnabled: true);

            return new TutorDetailsDto(tutor, studentId);
        }

        public async Task<bool> RemoveTutorAsync(long studentId, long tutorId)
        {
            var studentTutor = await studentTutorRepository.GetStudentTutorAsync(st => st.StudentId.Equals(studentId) && st.TutorId.Equals(tutorId));
            studentTutor.IsActive = false;

            return await studentTutorRepository.UpdateStudentTutorAsync(studentTutor);
        }

        private static Expression<Func<Tutor, bool>> GetExpressionToSearchedTutors(SearchedUserParameters parameters)
        {
            Expression<Func<Tutor, bool>> expression = r => r.Username.ToLower().Contains(parameters.Params.Trim().ToLower()) ||
                r.FirstName.ToLower().Contains(parameters.Params.Trim().ToLower()) ||
                r.LastName.ToLower().Contains(parameters.Params.Trim().ToLower());

            return expression;
        }
    }
}