using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<ICollection<User>> GetAllUsersAsync(bool isActiv);
    }
}
