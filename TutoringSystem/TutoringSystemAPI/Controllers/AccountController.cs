using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Dtos.Enums;
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
            var created = await userService.RegisterStudentAsync(model);
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
                return BadRequest(loginResult.Status);

            var token = jwtProvider.GenerateJwtToken(loginResult.User);
            Response.Headers.Add("Authorization", token);

            return Ok(loginResult.Status);
        }

        [SwaggerOperation(Summary = "Gets role of the currently logged user")]
        [HttpGet("role")]
        [Authorize(Roles = "Tutor,Student")]
        public ActionResult<Role> GetUserRole()
        {
            var role = User.FindFirst(c => c.Type == ClaimTypes.Role).Value;

            return Ok(role);
        }

        [SwaggerOperation(Summary = "Changes password of the currently logged user")]
        [HttpPost("password")]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> ChangePassword([FromBody] PasswordDto passwordModel)
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var changedErrors = await userService.ChangePasswordAsync(long.Parse(userId), passwordModel);

            if (changedErrors != null)
                return BadRequest(changedErrors);

            return Ok();
        }

        [SwaggerOperation(Summary = "Activates the account of the currently logged in user by actiavtion token")]
        [HttpPost("activate")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> ActivateAccountByToken(string token)
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var activated = await userService.ActivateUserByTokenAsync(long.Parse(userId), token);

            if (!activated)
                return BadRequest("Account could be not activated");

            return Ok();
        }

        [SwaggerOperation(Summary = "Sends a new activation token for the currently logged in user")]
        [HttpPost("newCode")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> SendNewActivationToken()
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var sent = await userService.SendNewActivationTokenAsync(long.Parse(userId));

            if (!sent)
                return BadRequest("New activation code could not be sent");

            return Ok();
        }

        [SwaggerOperation(Summary = "Deactivates the account of the currently logged in user")]
        [HttpDelete]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> DeactivateAccount()
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var deleted = await userService.DeactivateUserAsync(long.Parse(userId));

            if (!deleted)
                return BadRequest("Account could be not deleted");

            return NoContent();
        }
    }
}
