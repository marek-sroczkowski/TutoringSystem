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
            DeactivatePhones(user);
            await DeactivateOrdersAsync(user.Id);
            await DeactivateSubjectsAsync(user.Id);
            user.IsActiv = false;
            Update(user);

            return await SaveChangedAsync();
        }

        public async Task<User> GetUserAsync(Expression<Func<User, bool>> expression, bool? isActiv = true)
        {
            if (isActiv.HasValue)
                ExpressionMerger.MergeExpression(ref expression, u => u.IsActiv.Equals(isActiv.Value));

            var user = await DbContext.Users
                .Include(u => u.Contact)
                .Include(u => u.Address)
                .Include(u => u.ActivationTokens)
                .FirstOrDefaultAsync(expression);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsersCollectionAsync(Expression<Func<User, bool>> expression, bool? isActiv = true)
        {
            if (isActiv.HasValue)
                ExpressionMerger.MergeExpression(ref expression, u => u.IsActiv.Equals(isActiv.Value));

            var users = await FindByCondition(expression)
                .Include(u => u.Address)
                .Include(u => u.Contact)
                .ToListAsync();

            return users;
        }

        public async Task<bool> UpdateUser(User user)
        {
            Update(user);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateUsersCollection(IEnumerable<User> users)
        {
            DbContext.UpdateRange(users);

            return await SaveChangedAsync();
        }

        private async Task DeactivateSubjectsAsync(long userId)
        {
            await DbContext.Subjects.ForEachAsync(s =>
            {
                if (s.TutorId.Equals(userId))
                    s.IsActiv = false;
            });
        }

        private async Task DeactivateOrdersAsync(long userId)
        {
            await DbContext.AdditionalOrders.ForEachAsync(o =>
            {
                if (o.TutorId.Equals(userId))
                    o.IsActiv = false;
            });
        }

        private void DeactivatePhones(User user)
        {
            user.Contact.PhoneNumbers.ToList().ForEach(p => p.IsActiv = false);
        }
    }
}
