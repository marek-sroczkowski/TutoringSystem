using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IContactRepository
    {
        Task<bool> AddContactAsync(Contact contact);
        Task<Contact> GetContactByIdAsync(long contactId);
        Task<Contact> GetContactByUserIdAsync(long userId);
        Task<bool> UpdateContactAsync(Contact updatedContact);
        Task<bool> DeleteContactAsync(Contact contact);
        Task<IEnumerable<Contact>> GetContactsAsync(Expression<Func<Contact, bool>> expression);
    }
}
