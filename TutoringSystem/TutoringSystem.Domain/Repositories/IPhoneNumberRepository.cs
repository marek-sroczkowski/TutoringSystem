using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IPhoneNumberRepository
    {
        Task<bool> AddPhoneNumberAsync(PhoneNumber phoneNumber);
        Task<bool> AddPhoneNumbersCollectionAsync(IEnumerable<PhoneNumber> phoneNumbers);
        Task<PhoneNumber> GetPhoneNumberAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        IQueryable<PhoneNumber> GetPhoneNumbersCollection(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        Task<IEnumerable<PhoneNumber>> GetPhoneNumbersCollectionAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true, bool isEagerLoadingEnabled = false);
        bool PhoneNumberExists(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true);
        Task<bool> RemovePhoneNumberAsync(PhoneNumber phoneNumber);
        Task<bool> UpdatePhoneNumberAsync(PhoneNumber phoneNumber);
        Task<bool> UpdatePhoneNumbersCollectionAsync(IEnumerable<PhoneNumber> phoneNumbers);
    }
}