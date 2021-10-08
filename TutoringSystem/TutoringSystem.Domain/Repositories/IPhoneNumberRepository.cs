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
        Task<PhoneNumber> GetPhoneNumberById(long phoneNumberId, bool isActiv = true);
        Task<IEnumerable<PhoneNumber>> GetPhoneNumbersAsync(Expression<Func<PhoneNumber, bool>> expression, bool? isActiv = true);
    }
}
