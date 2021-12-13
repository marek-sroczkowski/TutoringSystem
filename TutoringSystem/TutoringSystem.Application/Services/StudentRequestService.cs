using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Dtos.StudentDtos;
using TutoringSystem.Application.Dtos.StudentRequestDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class StudentRequestService : IStudentRequestService
    {
        private readonly ITutorRepository tutorRepository;
        private readonly IStudentTutorRepository studentTutorRepository;
        private readonly IStudentTutorRequestRepository requestRepository;
        private readonly IStudentService studentService;
        private readonly IMapper mapper;

        public StudentRequestService(ITutorRepository tutorRepository,
            IStudentTutorRepository studentTutorRepository,
            IStudentTutorRequestRepository requestRepository,
            IStudentService studentService,
            IMapper mapper)
        {
            this.tutorRepository = tutorRepository;
            this.studentTutorRepository = studentTutorRepository;
            this.requestRepository = requestRepository;
            this.studentService = studentService;
            this.mapper = mapper;
        }

        public async Task<AddTutorToStudentStatus> AddRequestAsync(long studentId, long tutorId)
        {
            var existingStudentTutor = await studentTutorRepository.GetStudentTutorAsync(st => st.StudentId.Equals(studentId) && st.TutorId.Equals(tutorId));
            if (existingStudentTutor != null && existingStudentTutor.IsActive)
                return AddTutorToStudentStatus.TutorWasAlreadyAdded;

            var tutor = await tutorRepository.GetTutorAsync(s => s.Id.Equals(tutorId));
            if (tutor is null)
                return AddTutorToStudentStatus.IncorrectTutor;

            var request = await requestRepository.GetRequestAsync(r => r.StudentId.Equals(studentId) && r.TutorId.Equals(tutorId));
            if (request != null && request.IsActive)
                return AddTutorToStudentStatus.RequestWasAlreadyCreated;
            else if (request != null && !request.IsActive)
                return await ActivateRequestAsync(request);

            return await TryCreateRequest(studentId, tutorId);
        }

        public async Task<bool> AcceptRequest(long requestId, NewExistingStudentDto student)
        {
            var request = await requestRepository.GetRequestAsync(r => r.Id.Equals(requestId));
            if (!request.IsActive || request.IsAccepted || request.StudentId != student.StudentId)
                return false;

            request.IsAccepted = true;
            request.IsActive = false;

            return await requestRepository.UpdateRequestAsync(request) &&
                await studentService.AddStudentToTutorAsync(request.TutorId, student) == AddStudentToTutorStatus.Added;
        }

        public async Task<bool> DeclineRequest(long requestId)
        {
            var request = await requestRepository.GetRequestAsync(r => r.Id.Equals(requestId));
            if (!request.IsActive)
                return false;

            request.IsAccepted = false;
            request.IsActive = false;

            return await requestRepository.UpdateRequestAsync(request);
        }

        public async Task<IEnumerable<StudentRequestDto>> GetRequestsByStudentId(long studentId)
        {
            var requests = await requestRepository.GetRequestsCollectionAsync(r => r.StudentId.Equals(studentId) && r.IsActive && !r.IsAccepted);

            return mapper.Map<IEnumerable<StudentRequestDto>>(requests);
        }

        public async Task<IEnumerable<StudentRequestDto>> GetRequestsByTutorId(long tutorId)
        {
            var requests = await requestRepository.GetRequestsCollectionAsync(r => r.TutorId.Equals(tutorId) && r.IsActive && !r.IsAccepted);

            return mapper.Map<IEnumerable<StudentRequestDto>>(requests);
        }

        private async Task<AddTutorToStudentStatus> ActivateRequestAsync(StudentTutorRequest request)
        {
            request.IsActive = true;
            request.IsAccepted = false;
            request.CreatedDate = DateTime.Now;
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