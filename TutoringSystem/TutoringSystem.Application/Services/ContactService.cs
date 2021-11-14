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
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public ContactService(IContactRepository contactRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.contactRepository = contactRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<ContactDetailsDto> GetContactByIdAsync(long contactId)
        {
            var contact = await contactRepository.GetContactAsync(c => c.Id.Equals(contactId));
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(contact.UserId));
            var contactDto = mapper.Map<ContactDetailsDto>(contact);
            contactDto.Owner = $"{user.FirstName} {user.LastName}";

            return contactDto;
        }

        public async Task<ContactDto> GetContactByUserAsync(long userId)
        {
            var contact = await contactRepository.GetContactAsync(c => c.UserId.Equals(userId));

            return mapper.Map<ContactDto>(contact);
        }

        public async Task<bool> UpdateContactAsync(UpdatedContactDto updatedContact)
        {
            var existingContact = await contactRepository.GetContactAsync(c => c.Id.Equals(updatedContact.Id));
            var contact = mapper.Map(updatedContact, existingContact);

            return await contactRepository.UpdateContactAsync(contact);
        }
    }
}