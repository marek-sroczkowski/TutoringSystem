﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddSubjectAsync(Subject subject)
        {
            Create(subject);

            return await SaveChangedAsync();
        }

        public async Task<bool> DeleteSubjectAsync(Subject subject)
        {
            subject.IsActiv = false;
            Update(subject);

            return await SaveChangedAsync();
        }

        public async Task<Subject> GetSubjectByIdAsync(long subjectId)
        {
            var subject = await DbContext.Subjects
                .Include(s => s.Tutor)
                .FirstOrDefaultAsync(s => s.IsActiv && s.Id.Equals(subjectId));

            return subject;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync(Expression<Func<Subject, bool>> expression, bool? isActiv = true)
        {
            if (isActiv.HasValue)
                ExpressionMerger.MergeExpression(ref expression, s => s.IsActiv.Equals(isActiv.Value));

            var subjects = await FindByCondition(expression)
                .ToListAsync();

            return subjects;
        }

        public async Task<bool> UpdateSubjectAsync(Subject updatedSubject)
        {
            Update(updatedSubject);

            return await SaveChangedAsync();
        }
    }
}
