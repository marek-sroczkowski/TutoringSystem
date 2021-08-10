using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            user.IsActiv = false;
            dbContext.Users.Update(user);

            return (await dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<ICollection<User>> GetAllUsersAsync() => await dbContext.Users
            .Where(u => u.IsActiv)
            .ToListAsync();

        public async Task<User> GetUserByIdAsync(long id)
        {
            var user = await dbContext.Users
                .Where(u => u.IsActiv)
                .FirstOrDefaultAsync(u => u.Id.Equals(id));

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await dbContext.Users
                .Where(u => u.IsActiv)
                .FirstOrDefaultAsync(u => u.Username.Equals(username));

            return user;
        }

        public async Task<bool> UpdateUser(User user)
        {
            dbContext.Users.Update(user);
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
