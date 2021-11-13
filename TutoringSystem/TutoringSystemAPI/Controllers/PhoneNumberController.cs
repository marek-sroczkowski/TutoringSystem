using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.API.Filters.TypeFilters;

namespace TutoringSystem.API.Controllers
{
    [Route("/api/contact/{contactId}/phoneNumber")]
    [Authorize]
    [ApiController]
    public class PhoneNumberController : ControllerBase
    {
        private readonly IPhoneNumberService phoneNumberService;

        public PhoneNumberController(IPhoneNumberService phoneNumberService)
        {
            this.phoneNumberService = phoneNumberService;
        }

        [SwaggerOperation(Summary = "Adds a new phone to a specific contact")]
        [HttpPost]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateContactExistence]
        public async Task<ActionResult> AddPhones(long contactId, [FromBody] NewPhoneNumberDto model)
        {
            var createdPhone = await phoneNumberService.AddPhoneNumberAsync(contactId, model);
            if (createdPhone is null)
                return BadRequest("Phone could be not added");

            return Created($"api/contact/{contactId}/phoneNumber/{createdPhone.Id}", null);
        }

        [SwaggerOperation(Summary = "Retrieves phone numbers from a specific contact")]
        [HttpGet]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateContactExistence]
        public async Task<ActionResult<ICollection<PhoneNumberDto>>> GetPhones(long contactId)
        {
            var phones = await phoneNumberService.GetPhoneNumbersByContactIdAsync(contactId);

            return Ok(phones);
        }

        [SwaggerOperation(Summary = "Retrieves phone numbers from a specific contact")]
        [HttpGet("{phoneNumberId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateContactExistence]
        public async Task<ActionResult<PhoneNumberDto>> GetPhoneById(long phoneNumberId)
        {
            var phone = await phoneNumberService.GetPhoneNumberById(phoneNumberId);

            return Ok(phone);
        }

        [SwaggerOperation(Summary = "Updates a existing phone number")]
        [HttpPut]
        [Authorize(Roles = "Tutor, Student")]
        [ValidatePhoneNumberExistence]
        public async Task<ActionResult> UpdatePhone([FromBody] UpdatedPhoneNumberDto model)
        {
            var updated = await phoneNumberService.UpdatePhoneNumberAsync(model);
            if (!updated)
                return BadRequest("Phone could be not updated");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Removes a specific phone number")]
        [HttpDelete("{phoneNumberId}")]
        [Authorize(Roles = "Tutor, Student")]
        [ValidatePhoneNumberExistence]
        public async Task<ActionResult> RemovePhone(long phoneNumberId)
        {
            var removed = await phoneNumberService.DeletePhoneNumberAsync(phoneNumberId);
            if (!removed)
                return BadRequest("Phone number could be not removed");

            return NoContent();
        }
    }
}
