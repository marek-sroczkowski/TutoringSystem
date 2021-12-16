using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.TutorDtos;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Application.Parameters;
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
        private readonly IStudentTutorRequestRepository requestRepository;
        private readonly IMapper mapper;

        public TutorService(ITutorRepository tutorRepository,
            IStudentRepository studentRepository,
            IStudentTutorRepository studentTutorRepository,
            IStudentTutorRequestRepository requestRepository,
            IMapper mapper)
        {
            this.tutorRepository = tutorRepository;
            this.studentRepository = studentRepository;
            this.studentTutorRepository = studentTutorRepository;
            this.requestRepository = requestRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TutorDto>> GetTutorsByStudentIdAsync(long studentId)
        {
            var tutors = (await studentTutorRepository.GetStudentTuturCollectionAsync(st => st.StudentId.Equals(studentId)))
                .Select(st => st.Tutor);

            return tutors.Select(t => new TutorDto(t, studentId));
        }

        public async Task<PagedList<TutorSimpleDto>> GetTutors(SearchedUserParameters parameters)
        {
            var tutors = string.IsNullOrWhiteSpace(parameters.Params) ?
                new List<Tutor>() :
                await tutorRepository.GetTutorsCollectionAsync(GetExpressionToSearchedTutors(parameters));
            var tutorDtos = mapper.Map<ICollection<TutorSimpleDto>>(tutors);

            return PagedList<TutorSimpleDto>.ToPagedList(tutorDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<TutorDetailsDto> GetTutorAsync(long tutorId, long studentId)
        {
            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId));

            return new TutorDetailsDto(tutor, studentId);
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

        private Expression<Func<Tutor, bool>> GetExpressionToSearchedTutors(SearchedUserParameters parameters)
        {
            Expression<Func<Tutor, bool>> expression = r => r.Username.ToLower().Contains(parameters.Params.Trim().ToLower()) ||
                r.FirstName.ToLower().Contains(parameters.Params.Trim().ToLower()) ||
                r.LastName.ToLower().Contains(parameters.Params.Trim().ToLower());

            return expression;
        }

        private async Task<AddTutorToStudentStatus> ActivateRequestAsync(StudentTutorRequest request)
        {
            request.IsActive = true;
            request.IsAccepted = false;
            return await requestRepository.UpdateRequestAsync(request) ?
                AddTutorToStudentStatus.RequestCreated :
                AddTutorToStudentStatus.InternalError;
        }

        private async Task<AddTutorToStudentStatus> TryCreateRequest(long studentId, long tutorId)
        {
            return await requestRepository.AddRequestAsync(new StudentTutorRequest { StudentId = studentId, TutorId = tutorId }) ?
                AddTutorToStudentStatus.RequestCreated :
                AddTutorToStudentStatus.InternalError;
        }
    }
}