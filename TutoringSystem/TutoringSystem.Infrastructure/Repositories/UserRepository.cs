using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            user.IsActiv = false;
            Update(user);

            return await SaveChangedAsync();
        }

        public async Task<ICollection<User>> GetAllUsersAsync(bool isActiv)
        {
            var users = await FindByCondition(u => u.IsActiv.Equals(isActiv))
                .ToListAsync();

            return users;
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            var user = await DbContext.Users
                .Where(u => u.IsActiv)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));

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
