using AutoMapper;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository contactRepository;
        private readonly IMapper mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            this.contactRepository = contactRepository;
            this.mapper = mapper;
        }

        public async Task<ContactDetailsDto> GetContactByIdAsync(long contactId)
        {
            var contact = await contactRepository.GetContactAsync(c => c.Id.Equals(contactId));

            return mapper.Map<ContactDetailsDto>(contact);
        }

        public async Task<ContactDetailsDto> GetContactByUserAsync(long userId)
        {
            var contact = await contactRepository.GetContactAsync(c => c.UserId.Equals(userId));

            return mapper.Map<ContactDetailsDto>(contact);
        }

        public async Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact)
        {
            var existingContact = await contactRepository.GetContactAsync(c => c.Id.Equals(updatedContact.Id));
            var contact = mapper.Map(updatedContact, existingContact);

            return await contactRepository.UpdateContactAsync(contact);
        }
    }
}
