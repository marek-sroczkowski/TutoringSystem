using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.API.Filters.TypeFilters;
using TutoringSystem.Application.Authorization;
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
        private readonly IAuthorizationService authorizationService;

        public ContactController(IContactService contactService, IAuthorizationService authorizationService)
        {
            this.contactService = contactService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves contact of the current logged in user")]
        [HttpGet]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult<ContactDetailsDto>> GetContact()
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var contact = await contactService.GetContactByUserAsync(long.Parse(userId));

            return Ok(contact);
        }

        [SwaggerOperation(Summary = "Updates a existing contact")]
        [HttpPut]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateContactExistence]
        public async Task<ActionResult> UpdateContact([FromBody] UpdatedContactDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorizationResult = authorizationService.AuthorizeAsync(User, model, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var updated = await contactService.UpdateContactAsync(model);
            if (!updated)
                return BadRequest("Contact could be not updated");

            return NoContent();
        }
    }
}
