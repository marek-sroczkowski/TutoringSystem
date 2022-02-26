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

        public async Task<bool> AddUserAsync(User user)
        {
            Create(user);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddUsersCollectionAsync(IEnumerable<User> users)
        {
            CreateRange(users);

            return await SaveChangedAsync();
        }

        public async Task<User> GetUserAsync(Expression<Func<User, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, u => u.IsActive.Equals(isActive.Value));
            }

            var user = isEagerLoadingEnabled
                ? await GetUserWithEagerLoadingAsync(expression)
                : await GetUserWithoutEagerLoadingAsync(expression);

            return user;
        }

        public IQueryable<User> GetUsersCollection(Expression<Func<User, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, u => u.IsActive.Equals(isActive.Value));
            }

            var users = isEagerLoadingEnabled
                ? GetUsersCollectionWithEagerLoading(expression)
                : Find(expression);

            return users;
        }

        public bool IsUserExist(Expression<Func<User, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, u => u.IsActive.Equals(isActive.Value));
            }

            var exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveUserAsync(User user)
        {
            DeactivatePhones(user);
            await DeactivateOrdersAsync(user.Id);
            await DeactivateSubjectsAsync(user.Id);

            user.IsActive = false;
            user.IsEnable = false;
            Update(user);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            Update(user);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateUsersCollectionAsync(IEnumerable<User> users)
        {
            UpdateRange(users);

            return await SaveChangedAsync();
        }

        private async Task DeactivateSubjectsAsync(long userId)
        {
            await DbContext.Subjects.ForEachAsync(s =>
            {
                if (s.TutorId.Equals(userId))
                {
                    s.IsActive = false;
                }
            });
        }

        private async Task DeactivateOrdersAsync(long userId)
        {
            await DbContext.AdditionalOrders.ForEachAsync(o =>
            {
                if (o.TutorId.Equals(userId))
                {
                    o.IsActive = false;
                }
            });
        }

        private void DeactivatePhones(User user)
        {
            var phones = DbContext.PhoneNumbers.Where(p => p.ContactId.Equals(user.Contact.Id)).ToList();
            phones.ForEach(p => p.IsActive = false);
        }

        private async Task<User> GetUserWithEagerLoadingAsync(Expression<Func<User, bool>> expression)
        {
            var user = await Find(expression)
                .Include(u => u.Contact)
                .Include(u => u.Address)
                .Include(u => u.ActivationTokens)
                .FirstOrDefaultAsync();

            return user;
        }

        private async Task<User> GetUserWithoutEagerLoadingAsync(Expression<Func<User, bool>> expression)
        {
            var user = await Find(expression)
                .FirstOrDefaultAsync();

            return user;
        }

        private IQueryable<User> GetUsersCollectionWithEagerLoading(Expression<Func<User, bool>> expression)
        {
            var users = Find(expression)
                .Include(u => u.Contact)
                .Include(u => u.Address)
                .Include(u => u.ActivationTokens);

            return users;
        }
    }
}