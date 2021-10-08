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
        Task<bool> UpdateContactAsync(Contact updatedContact);
        Task<bool> DeleteContactAsync(Contact contact);
        Task<Contact> GetContactAsync(Expression<Func<Contact, bool>> expression);
        Task<IEnumerable<Contact>> GetContactsCollectionAsync(Expression<Func<Contact, bool>> expression);
    }
}
