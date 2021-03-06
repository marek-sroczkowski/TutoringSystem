using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ContactDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactDetailsDto> GetContactByIdAsync(long contactId);
        Task<ContactDto> GetContactByUserAsync(long userId);
        Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact);
    }
}