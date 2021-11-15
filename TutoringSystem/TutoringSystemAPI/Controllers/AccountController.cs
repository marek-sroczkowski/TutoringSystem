using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.API.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountController: ControllerBase
    {
        private readonly IUserService userService;
        private readonly IJwtProvider jwtProvider;

        public AccountController(IUserService userService, IJwtProvider jwtProvider)
        {
            this.userService = userService;
            this.jwtProvider = jwtProvider;
        }

        [SwaggerOperation(Summary = "Creates a new tutor")]
        [HttpPost("register/tutor")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterTutor([FromBody] RegisterTutorDto model)
        {
            var created = await userService.RegisterTutorAsync(model);
            if (!created)
                return BadRequest("New tutor could not be added");

            return Ok();
        }

        [SwaggerOperation(Summary = "Creates a new student")]
        [HttpPost("register/student")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> RegisterStudent([FromBody] RegisterStudentDto model)
        {
            var created = await userService.RegisterStudentAsync(User.GetUserId(), model);
            if (!created)
                return BadRequest("New student could not be added");

            return Ok();
        }

        [SwaggerOperation(Summary = "Generates a token when logging in successfully")]
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginStatus>> Login([FromBody] LoginUserDto model)
        {
            var loginResult = await userService.TryLoginAsync(model);
            if (loginResult.Status.Equals(LoginStatus.InvalidUsernameOrPassword))
                return Ok(loginResult.Status);

            var token = jwtProvider.GenerateJwtToken(loginResult.User);
            Response.Headers.Add("Authorization", token);

            return Ok(loginResult.Status);
        }

        [SwaggerOperation(Summary = "Gets role of the currently logged user")]
        [HttpGet("role")]
        [Authorize(Roles = "Tutor,Student")]
        public ActionResult<Role> GetUserRole()
        {
            var role = User.GetUserRole();

            return Ok(role);
        }

        [SwaggerOperation(Summary = "Changes password of the currently logged user")]
        [HttpPatch("password")]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> ChangePassword([FromBody] PasswordDto passwordModel)
        {
            var changedErrors = await userService.ChangePasswordAsync(User.GetUserId(), passwordModel);
            if (changedErrors != null)
                return BadRequest(changedErrors);

            return Ok();
        }

        [SwaggerOperation(Summary = "Activates the account of the currently logged in user by actiavtion token")]
        [HttpPost("activate")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> ActivateAccountByToken(string token)
        {
            var activated = await userService.ActivateUserByTokenAsync(User.GetUserId(), token);
            if (!activated)
                return BadRequest("Account could be not activated");

            return Ok();
        }

        [SwaggerOperation(Summary = "Sends a new activation token for the currently logged in user")]
        [HttpPost("newCode")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> SendNewActivationToken()
        {
            var sent = await userService.SendNewActivationTokenAsync(User.GetUserId());
            if (!sent)
                return BadRequest("New activation code could not be sent");

            return Ok();
        }

        [SwaggerOperation(Summary = "Deactivates the account of the currently logged in user")]
        [HttpDelete]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> DeactivateAccount()
        {
            var deleted = await userService.DeactivateUserAsync(User.GetUserId());
            if (!deleted)
                return BadRequest("Account could be not deleted");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Sets profile picture for the currently logged in user")]
        [HttpPatch("image")]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> SetProfileImage([FromBody] string imageBase64)
        {
            var set = await userService.SetProfileImageAsync(User.GetUserId(), imageBase64);
            if (!set)
                return BadRequest("Picture could be not set");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Removes profile picture for the currently logged in user")]
        [HttpDelete("image")]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> RemoveProfileImage()
        {
            var deleted = await userService.RemoveProfilePictureAsync(User.GetUserId());
            if (!deleted)
                return BadRequest("Picture could be not deleted");

            return NoContent();
        }
    }
}
