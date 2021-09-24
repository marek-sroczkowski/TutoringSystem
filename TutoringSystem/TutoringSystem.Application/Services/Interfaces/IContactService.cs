using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ContactDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IContactService
    {
        Task<bool> DeleteContactAsync(long contactId);
        Task<ContactDetailsDto> GetContactByIdAsync(long contactId);
        Task<ContactDetailsDto> GetContactByUserAsync(long userId);
        Task<bool> UpdateContactAsync(long userId, UpdatedContactDto updatedContact);
    }
}