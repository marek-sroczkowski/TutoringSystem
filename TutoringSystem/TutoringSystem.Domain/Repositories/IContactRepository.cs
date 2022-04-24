using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IContactRepository
    {
        Task<bool> AddContactAsync(Contact contact);
        Task<bool> AddContactsCollection(IEnumerable<Contact> contacts);
        Task<Contact> GetContactAsync(Expression<Func<Contact, bool>> expression, bool isEagerLoadingEnabled = false);
        IQueryable<Contact> GetContactsCollection(Expression<Func<Contact, bool>> expression, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<Contact>> GetContactsCollectionAsync(Expression<Func<Contact, bool>> expression, bool isEagerLoadingEnabled = false);
        bool IsContactExist(Expression<Func<Contact, bool>> expression);
        Task<bool> RemoveContactAsync(Contact contact);
        Task<bool> UpdateContactAsync(Contact updatedContact);
        Task<bool> UpdateContactsCollectionAsync(IEnumerable<Contact> contacts);
    }
}