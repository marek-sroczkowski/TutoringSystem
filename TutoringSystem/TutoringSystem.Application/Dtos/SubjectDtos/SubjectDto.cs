﻿using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.SubjectDtos
{
    public class SubjectDto : IMap
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public SubjectCategory Category { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Subject, SubjectDto>();
        }
    }
}