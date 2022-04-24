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

        public async Task<bool> AddPhoneNumbersCollectionAsync(IEnumerable<PhoneNumber> phoneNumbers)
        {
            CreateRange(phoneNumbers);

            return await SaveChangedAsync();
        }

        public async Task<PhoneNumber> GetPhoneNumberAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, p => p.IsActive.Equals(isActive.Value));
            }

            var phone = isEagerLoadingEnabled
                ? await GetNumberWithEagerLoadingAsync(expression)
                : await GetNumberWithoutEagerLoadingAsync(expression);

            return phone;
        }

        public IQueryable<PhoneNumber> GetPhoneNumbersCollection(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, p => p.IsActive.Equals(isActive.Value));
            }

            var phones = isEagerLoadingEnabled
                ? GetNumbersCollectionWithEagerLoading(expression)
                : Find(expression);

            return phones;
        }

        public async Task<IEnumerable<PhoneNumber>> GetPhoneNumbersCollectionAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, p => p.IsActive.Equals(isActive.Value));
            }

            var phones = isEagerLoadingEnabled
                ? GetNumbersCollectionWithEagerLoading(expression)
                : Find(expression);

            return await phones.ToListAsync();
        }

        public bool PhoneNumberExists(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true)
        {
            if (isActive.HasValue)
            {
                ExpressionMerger.MergeExpression(ref expression, r => r.IsActive.Equals(isActive.Value));
            }

            bool exist = Contains(expression);

            return exist;
        }

        public async Task<bool> RemovePhoneNumberAsync(PhoneNumber phoneNumber)
        {
            phoneNumber.IsActive = false;

            return await UpdatePhoneNumberAsync(phoneNumber);
        }

        public async Task<bool> UpdatePhoneNumberAsync(PhoneNumber phoneNumber)
        {
            Update(phoneNumber);

            return await SaveChangedAsync();
        }

        public async Task<bool> UpdatePhoneNumbersCollectionAsync(IEnumerable<PhoneNumber> phoneNumbers)
        {
            UpdateRange(phoneNumbers);

            return await SaveChangedAsync();
        }

        private async Task<PhoneNumber> GetNumberWithEagerLoadingAsync(Expression<Func<PhoneNumber, bool>> expression)
        {
            var number = await Find(expression)
                .Include(p => p.Contact)
                .FirstOrDefaultAsync();

            return number;
        }

        private async Task<PhoneNumber> GetNumberWithoutEagerLoadingAsync(Expression<Func<PhoneNumber, bool>> expression)
        {
            var nummber = await Find(expression)
                .FirstOrDefaultAsync();

            return nummber;
        }

        private IQueryable<PhoneNumber> GetNumbersCollectionWithEagerLoading(Expression<Func<PhoneNumber, bool>> expression)
        {
            var numbers = Find(expression)
                .Include(p => p.Contact);

            return numbers;
        }
    }
}
