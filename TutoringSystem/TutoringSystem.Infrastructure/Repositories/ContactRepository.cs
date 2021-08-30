using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            var contact = await DbContext.Contacts
                .Include(c => c.User)
                .Include(c => c.PhoneNumbers)
                .FirstOrDefaultAsync(c => c.Id.Equals(contactId));

            return contact;
        }

        public async Task<Contact> GetContactByUserIdAsync(int userId)
        {
            var contact = await DbContext.Contacts
                .Include(c => c.User)
                .Include(c => c.PhoneNumbers)
                .FirstOrDefaultAsync(c => c.UserId.Equals(userId));

            return contact;
        }

        public async Task<bool> UpdateContactAsync(Contact updatedContact)
        {
            Update(updatedContact);

            return await SaveChangedAsync();
        }
    }
}
