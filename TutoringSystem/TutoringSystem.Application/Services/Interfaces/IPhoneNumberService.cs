using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.PhoneNumber;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IPhoneNumberService
    {
        Task<PhoneNumberDto> AddPhoneNumberAsync(long contactId, NewPhoneNumberDto phoneNumber);
        Task<bool> RemovePhoneNumberAsync(long phoneNumberId);
        Task<PhoneNumberDetailsDto> GetPhoneNumberById(long phoneNumberId);
        Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByContactIdAsync(long contactId);
        Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByUserAsync(long userId);
        Task<bool> UpdatePhoneNumberAsync(UpdatedPhoneNumberDto updatedPhoneNumber);
    }
}