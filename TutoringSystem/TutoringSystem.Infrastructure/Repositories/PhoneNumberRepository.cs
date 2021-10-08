using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Application.Helpers;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;
using TutoringSystem.Infrastructure.Data;

namespace TutoringSystem.Infrastructure.Repositories
{
    public class PhoneNumberRepository : RepositoryBase<PhoneNumber>, IPhoneNumberRepository
    {
        public PhoneNumberRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddPhoneNumberAsync(PhoneNumber phoneNumber)
        {
            Create(phoneNumber);

            return await SaveChangedAsync();
        }

        public async Task<bool> AddPhoneNumbersAsync(ICollection<PhoneNumber> phoneNumbers)
        {
            DbContext.PhoneNumbers.AddRange(phoneNumbers);

            return await SaveChangedAsync();
        }

        public async Task<PhoneNumber> GetPhoneNumberById(long phoneNumberId, bool isActiv = true)
        {
            var phone = await DbContext.PhoneNumbers
                .Include(p => p.Contact)
                .FirstOrDefaultAsync(p => p.Id.Equals(phoneNumberId) && p.IsActiv.Equals(isActiv));

            return phone;
        }

        public async Task<IEnumerable<PhoneNumber>> GetPhoneNumbersAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActiv = true)
        {
            if (isActiv.HasValue)
                ExpressionMerger.MergeExpression(ref expression, p => p.IsActiv.Equals(isActiv.Value));

            var phones = FindByCondition(expression);

            return await phones.ToListAsync();
        }

        public async Task<bool> DeletePhoneNumberAsync(PhoneNumber phoneNumber)
        {
            phoneNumber.IsActiv = false;

            return await UpdatePhoneNumberAsync(phoneNumber);
        }

        public async Task<bool> UpdatePhoneNumberAsync(PhoneNumber phoneNumber)
        {
            Update(phoneNumber);

            return await SaveChangedAsync();
        }
    }
}
