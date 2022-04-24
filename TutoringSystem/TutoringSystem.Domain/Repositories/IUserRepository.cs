using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User user);
        Task<bool> AddUsersCollectionAsync(IEnumerable<User> users);
        Task<User> GetUserAsync(Expression<Func<User, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<User> GetUsersCollection(Expression<Func<User, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<User>> GetUsersCollectionAsync(Expression<Func<User, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool UserExists(Expression<Func<User, bool>> expression, bool? isActive = true);
        Task<bool> RemoveUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> UpdateUsersCollectionAsync(IEnumerable<User> users);
    }
}