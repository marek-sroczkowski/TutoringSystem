﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User> GetUserAsync(Expression<Func<User, bool>> expression, bool? isActiv = true);
        Task<IEnumerable<User>> GetUsersCollectionAsync(Expression<Func<User, bool>> expression, bool? isActiv = true);
    }
}
