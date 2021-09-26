using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.AddressDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.API.Filters;

namespace TutoringSystem.API.Controllers
{
    [Route("api/address")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;
        private readonly IAuthorizationService authorizationService;

        public AddressController(IAddressService addressService, IAuthorizationService authorizationService)
        {
            this.addressService = addressService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Retrieves address of the current logged in user")]
        [HttpGet]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult<AddressDetailsDto>> GetAddress()
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var address = await addressService.GetAddressByUserAsync(long.Parse(userId));

            return Ok(address);
        }

        [SwaggerOperation(Summary = "Updates a existing address")]
        [HttpPut]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateAddressExistence]
        public async Task<ActionResult> UpdateAddress([FromBody] UpdatedAddressDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorizationResult = authorizationService.AuthorizeAsync(User, model, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var updated = await addressService.UpdateAddressAsync(model);
            if (!updated)
                return BadRequest("Contact could be not updated");

            return NoContent();
        }
    }
}
