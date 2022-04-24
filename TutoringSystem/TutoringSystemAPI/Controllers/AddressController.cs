using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.API.Filters.Action;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Address;

namespace TutoringSystem.API.Controllers
{
    [Route("api/address")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService addressService;

        public AddressController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [SwaggerOperation(Summary = "Retrieves address of the current logged in user")]
        [HttpGet]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var address = await addressService.GetAddressByUserAsync(User.GetUserId());

            return Ok(address);
        }

        [SwaggerOperation(Summary = "Retrieves a specific address by unique id")]
        [HttpGet("{addressId}")]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult<AddressDetailsDto>> GetAddressById(long addressId)
        {
            var address = await addressService.GetAddressByIdAsync(addressId);

            return Ok(address);
        }

        [SwaggerOperation(Summary = "Updates a existing address")]
        [HttpPut]
        [Authorize(Roles = "Tutor, Student")]
        [ValidateAddressExistence]
        public async Task<ActionResult> UpdateAddress([FromBody] UpdatedAddressDto model)
        {
            var updated = await addressService.UpdateAddressAsync(model);
            if (!updated)
                return BadRequest("Address could be not updated");

            return NoContent();
        }
    }
}
