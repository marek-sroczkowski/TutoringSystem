using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Domain.Repositories
{
    public interface IPhoneNumberRepository
    {
        Task<bool> AddPhoneNumberAsync(PhoneNumber phoneNumber);
        Task<bool> UpdatePhoneNumberAsync(PhoneNumber phoneNumber);
        Task<bool> DeletePhoneNumberAsync(PhoneNumber phoneNumber);
        Task<bool> AddPhoneNumbersAsync(ICollection<PhoneNumber> phoneNumbers);
        Task<PhoneNumber> GetPhoneNumberAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true);
        Task<IEnumerable<PhoneNumber>> GetPhoneNumbersCollectionAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActive = true);
    }
}
