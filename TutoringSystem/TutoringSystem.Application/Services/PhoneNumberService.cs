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

        public async Task<bool> AddPhoneNumbersAsync(long userId, ICollection<NewPhoneNumberDto> phoneNumbers)
        {
            var contact = await contactRepository.GetContactByUserIdAsync(userId);
            var phones = mapper.Map<List<PhoneNumber>>(phoneNumbers);
            phones.ForEach(p => p.ContactId = contact.Id);

            return await phoneNumberRepository.AddPhoneNumbersAsync(phones);
        }

        public async Task<ICollection<PhoneNumberDto>> GetPhoneNumbersByUserAsync(long userId)
        {
            var contact = await contactRepository.GetContactByUserIdAsync(userId);

            return mapper.Map<ICollection<PhoneNumberDto>>(contact.PhoneNumbers);
        }

        public async Task<PhoneNumberDetailsDto> GetPhoneNumberById(long phoneNumberId)
        {
            var phone = await phoneNumberRepository.GetPhoneNumberById(phoneNumberId);

            return mapper.Map<PhoneNumberDetailsDto>(phone);
        }

        public async Task<bool> UpdatePhoneNumberAsync(UpdatedPhoneNumberDto updatedPhoneNumber)
        {
            var existingPhone = await phoneNumberRepository.GetPhoneNumberById(updatedPhoneNumber.Id);
            var phone = mapper.Map(updatedPhoneNumber, existingPhone);

            return await phoneNumberRepository.UpdatePhoneNumberAsync(phone);
        }

        public async Task<bool> ActivatePhoneNumberAsync(long phoneNumberId)
        {
            var phone = await phoneNumberRepository.GetPhoneNumberById(phoneNumberId, false);
            phone.IsActiv = true;

            return await phoneNumberRepository.UpdatePhoneNumberAsync(phone);
        }

        public async Task<bool> DeletePhoneNumberAsync(long phoneNumberId)
        {
            var phone = await phoneNumberRepository.GetPhoneNumberById(phoneNumberId);

            return await phoneNumberRepository.DeletePhoneNumberAsync(phone);
        }
    }
}
