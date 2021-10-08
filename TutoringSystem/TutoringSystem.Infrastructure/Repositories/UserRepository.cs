using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            user.Contact.PhoneNumbers.ToList().ForEach(p => p.IsActiv = false);
            user.IsActiv = false;
            Update(user);

            return await SaveChangedAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> expression, bool? isActiv = true)
        {
            if (isActiv.HasValue)
                ExpressionMerger.MergeExpression(ref expression, u => u.IsActiv.Equals(isActiv.Value));

            var users = await FindByCondition(expression)
                .ToListAsync();

            return users;
        }

        public async Task<User> GetUserByIdAsync(long userId, bool isActiv = true)
        {
            var user = await DbContext.Users
                .Where(u => u.IsActiv.Equals(isActiv))
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await DbContext.Users
                .Where(u => u.IsActiv)
                .FirstOrDefaultAsync(u => u.Username.Equals(username));

            return user;
        }

        public async Task<bool> UpdateUser(User user)
        {
            Update(user);

            return await SaveChangedAsync();
        }
    }
}
