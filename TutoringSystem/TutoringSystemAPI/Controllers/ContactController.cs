using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/contact")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;

        public ContactController(IContactService contactService)
        {
            this.contactService = contactService;
        }

        [SwaggerOperation(Summary = "Retrieves contact of the current logged in user")]
        [HttpGet]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult<ContactDto>> GetContact()
        {
            var contact = await contactService.GetContactByUserAsync(User.GetUserId());

            return Ok(contact);
        }

        [SwaggerOperation(Summary = "Retrieves a specific contact by unique id")]
        [HttpGet("{contactId}")]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult<ContactDetailsDto>> GetContactById(long contactId)
        {
            var contact = await contactService.GetContactByIdAsync(contactId);

            return Ok(contact);
        }

        [SwaggerOperation(Summary = "Updates a existing contact")]
        [HttpPut]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateContactExistence]
        public async Task<ActionResult> UpdateContact([FromBody] UpdatedContactDto model)
        {
            var updated = await contactService.UpdateContactAsync(model);

            return updated ? NoContent() : BadRequest("Contact could be not updated");
        }
    }
}