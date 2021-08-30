using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IContactRepository
    {
        Task<bool> AddContactAsync(Contact contact);
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<Contact> GetContactByUserIdAsync(int userId);
        Task<bool> UpdateContactAsync(Contact updatedContact);
        Task<bool> DeleteContactAsync(Contact contact);
    }
}
