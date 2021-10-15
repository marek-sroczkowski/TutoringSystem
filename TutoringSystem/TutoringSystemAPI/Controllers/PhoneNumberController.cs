﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Authorization;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.API.Filters.TypeFilters;
using TutoringSystem.Application.Extensions;

namespace TutoringSystem.API.Controllers
{
    [Route("/api/contact/phoneNumber")]
    [Authorize]
    [ApiController]
    public class PhoneNumberController : ControllerBase
    {
        private readonly IPhoneNumberService phoneNumberService;
        private readonly IAuthorizationService authorizationService;

        public PhoneNumberController(IPhoneNumberService phoneNumberService, IAuthorizationService authorizationService)
        {
            this.phoneNumberService = phoneNumberService;
            this.authorizationService = authorizationService;
        }

        [SwaggerOperation(Summary = "Adds new phones to the contacts of the currently logged in user")]
        [HttpPost]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult> AddPhones([FromBody] ICollection<NewPhoneNumberDto> model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.GetUserId();
            var added = await phoneNumberService.AddPhoneNumbersAsync(userId, model);
            if (!added)
                return BadRequest("Phones could be not created");

            return Created("api/contact/phoneNumber", null);
        }

        [SwaggerOperation(Summary = "Retrieves phone numbers of the current logged in user")]
        [HttpGet]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult<ICollection<PhoneNumberDto>>> GetPhones()
        {
            var userId = User.GetUserId();
            var phones = await phoneNumberService.GetPhoneNumbersByUserAsync(userId);

            return Ok(phones);
        }

        [SwaggerOperation(Summary = "Updates a existing phone number")]
        [HttpPut]
        [Authorize(Roles = "Tutor, Student")]
        [ValidatePhoneNumberExistence]
        public async Task<ActionResult> UpdatePhone([FromBody] UpdatedPhoneNumberDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var phone = await phoneNumberService.GetPhoneNumberById(model.Id);
            var authorizationResult = authorizationService.AuthorizeAsync(User, phone, new ResourceOperationRequirement(OperationType.Update)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

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
            var phone = await phoneNumberService.GetPhoneNumberById(phoneNumberId);
            var authorizationResult = authorizationService.AuthorizeAsync(User, phone, new ResourceOperationRequirement(OperationType.Delete)).Result;
            if (!authorizationResult.Succeeded)
                return Forbid();

            var removed = await phoneNumberService.DeletePhoneNumberAsync(phoneNumberId);
            if (!removed)
                return BadRequest("Tutor could be not removed from student's tutor list");

            return NoContent();
        }
    }
}
