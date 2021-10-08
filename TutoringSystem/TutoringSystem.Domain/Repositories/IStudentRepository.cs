﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IStudentRepository
    {
        Task<bool> AddStudentAsycn(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<Student> GetStudentAsync(Expression<Func<Student, bool>> expression, bool? isActiv = true);
        Task<IEnumerable<Student>> GetStudentsCollectionAsync(Expression<Func<Student, bool>> expression, bool? isActiv = true);
    }
}
