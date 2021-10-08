using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IPhoneNumberService
    {
        Task<bool> AddPhoneNumbersAsync(long userId, ICollection<NewPhoneNumberDto> phoneNumbers);
        Task<bool> DeletePhoneNumberAsync(long phoneNumberId);
        Task<PhoneNumberDetailsDto> GetPhoneNumberById(long phoneNumberId);
        Task<ICollection<PhoneNumberDto>> GetPhoneNumbersByUserAsync(long userId);
        Task<bool> UpdatePhoneNumberAsync(UpdatedPhoneNumberDto updatedPhoneNumber);
    }
}