﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.SubjectDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface ISubjectService
    {
        Task<SubjectDto> AddSubjectAsync(long tutorId, NewSubjectDto newSubjectModel);
        Task<bool> DeactivateSubjectAsync(long subjectId);
        Task<bool> ActivateSubjectAsync(long tutorId, long subjectId);
        Task<SubjectDetailsDto> GetSubjectByIdAsync(long subjectId);
        Task<ICollection<SubjectDto>> GetTutorSubjectsAsync(long tutorId, bool isActiv = true);
        Task<bool> UpdateSubjectAsync(long subjectId, UpdatedSubjectDto updatedSubject);
    }
}