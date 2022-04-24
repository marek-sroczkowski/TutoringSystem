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
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddContactAsync(Contact contact)
        {
            Create(contact);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddContactsCollection(IEnumerable<Contact> contacts)
        {
            CreateRange(contacts);

            return await SaveChangedAsync();
        }

        public async Task<Contact> GetContactAsync(Expression<Func<Contact, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            var contact = isEagerLoadingEnabled
                ? await GetContactWithEagerLoadingAsync(expression)
                : await GetContactWithoutEagerLoadingAsync(expression);

            return contact;
        }

        public IQueryable<Contact> GetContactsCollection(Expression<Func<Contact, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            ExpressionMerger.MergeExpression(ref expression, c => c.User.IsActive);

            var contacts = isEagerLoadingEnabled
                ? GetContactsCollectionWithEagerLoading(expression)
                : Find(expression);

            return contacts;
        }

        public async Task<IEnumerable<Contact>> GetContactsCollectionAsync(Expression<Func<Contact, bool>> expression, bool isEagerLoadingEnabled = false)
        {
            ExpressionMerger.MergeExpression(ref expression, c => c.User.IsActive);

            var contacts = isEagerLoadingEnabled
                ? GetContactsCollectionWithEagerLoading(expression)
                : Find(expression);

            return await contacts.ToListAsync();
        }

        public bool IsContactExist(Expression<Func<Contact, bool>> expression)
        {
            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemoveContactAsync(Contact contact)
        {
            Delete(contact);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateContactAsync(Contact contact)
        {
            Update(contact);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdateContactsCollectionAsync(IEnumerable<Contact> contacts)
        {
            UpdateRange(contacts);

            return await SaveChangedAsync();
        }

        private async Task<Contact> GetContactWithEagerLoadingAsync(Expression<Func<Contact, bool>> expression)
        {
            var contact = await Find(expression)
                .Include(c => c.User)
                .Include(c => c.PhoneNumbers.Where(p => p.IsActive))
                .FirstOrDefaultAsync();

            return contact;
        }

        private async Task<Contact> GetContactWithoutEagerLoadingAsync(Expression<Func<Contact, bool>> expression)
        {
            var contact = await Find(expression)
                .FirstOrDefaultAsync();

            return contact;
        }

        private IQueryable<Contact> GetContactsCollectionWithEagerLoading(Expression<Func<Contact, bool>> expression)
        {
            var contacts = Find(expression)
                .Include(c => c.User)
                .Include(c => c.PhoneNumbers.Where(p => p.IsActive));

            return contacts;
        }
    }
}