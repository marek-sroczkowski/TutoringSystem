using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly IContactRepository contactRepository;
        private readonly IPhoneNumberRepository phoneNumberRepository;
        private readonly IMapper mapper;

        public PhoneNumberService(IContactRepository contactRepository, IPhoneNumberRepository phoneNumberRepository, IMapper mapper)
        {
            this.contactRepository = contactRepository;
            this.phoneNumberRepository = phoneNumberRepository;
            this.mapper = mapper;
        }

        public async Task<PhoneNumberDto> AddPhoneNumberAsync(long contactId, NewPhoneNumberDto phoneNumber)
        {
            var phone = mapper.Map<PhoneNumber>(phoneNumber);
            phone.ContactId = contactId;

            return await phoneNumberRepository.AddPhoneNumberAsync(phone) ? mapper.Map<PhoneNumberDto>(phone) : null;
        }

        public async Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByUserAsync(long userId)
        {
            var contact = await contactRepository.GetContactAsync(c => c.UserId.Equals(userId));

            return mapper.Map<IEnumerable<PhoneNumberDto>>(contact.PhoneNumbers);
        }

        public async Task<IEnumerable<PhoneNumberDto>> GetPhoneNumbersByContactIdAsync(long contactId)
        {
            var contact = await contactRepository.GetContactAsync(c => c.Id.Equals(contactId));

            return mapper.Map<IEnumerable<PhoneNumberDto>>(contact.PhoneNumbers);
        }

        public async Task<PhoneNumberDetailsDto> GetPhoneNumberById(long phoneNumberId)
        {
            var phone = await phoneNumberRepository.GetPhoneNumberAsync(p => p.Id.Equals(phoneNumberId));

            return mapper.Map<PhoneNumberDetailsDto>(phone);
        }

        public async Task<bool> UpdatePhoneNumberAsync(UpdatedPhoneNumberDto updatedPhoneNumber)
        {
            var existingPhone = await phoneNumberRepository.GetPhoneNumberAsync(p => p.Id.Equals(updatedPhoneNumber.Id));
            var phone = mapper.Map(updatedPhoneNumber, existingPhone);

            return await phoneNumberRepository.UpdatePhoneNumberAsync(phone);
        }

        public async Task<bool> RemovePhoneNumberAsync(long phoneNumberId)
        {
            var phone = await phoneNumberRepository.GetPhoneNumberAsync(p => p.Id.Equals(phoneNumberId));

            return await phoneNumberRepository.RemovePhoneNumberAsync(phone);
        }
    }
}