﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeleteContactAsync(Contact contact)
        {
            Delete(contact);

            return await SaveChangedAsync();
        }

        public async Task<Contact> GetContactAsync(Expression<Func<Contact, bool>> expression)
        {
            var contact = await DbContext.Contacts
                .Include(c => c.User)
                .Include(c => c.PhoneNumbers.Where(p => p.IsActiv))
                .FirstOrDefaultAsync(expression);

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetContactsCollectionAsync(Expression<Func<Contact, bool>> expression)
        {
            ExpressionMerger.MergeExpression(ref expression, c => c.User.IsActiv);
            var contacts = FindByCondition(expression)
                .Include(c => c.User);

            return await contacts.ToListAsync();
        }

        public async Task<bool> UpdateContactAsync(Contact updatedContact)
        {
            Update(updatedContact);

            return await SaveChangedAsync();
        }
    }
}
