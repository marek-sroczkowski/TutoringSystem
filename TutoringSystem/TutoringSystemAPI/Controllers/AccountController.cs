using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Extensions;
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

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [SwaggerOperation(Summary = "Registers a new tutor")]
        [HttpPost("register/tutor")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterTutor([FromBody] RegisteredTutorDto model)
        {
            var createdTutor = await userService.RegisterTutorAsync(model);
            bool created = createdTutor != null && await userService.SendNewActivationTokenAsync(createdTutor.Id);

            return created ? Ok() : BadRequest("New tutor could not be registered");
        }

        [SwaggerOperation(Summary = "Creates a new student")]
        [HttpPost("create/student")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult> CreateStudent([FromBody] NewStudentDto model)
        {
            var created = await userService.CreateNewStudentAsync(User.GetUserId(), model);

            return created ? Ok() : BadRequest("New student could not be added");
        }

        [SwaggerOperation(Summary = "Registers a new student")]
        [HttpPost("register/student")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult> RegisterStudent([FromBody] RegisteredStudentDto model)
        {
            var createdStudent = await userService.RegisterStudentAsync(User.GetUserId(), model);
            bool created = createdStudent != null && await userService.SendNewActivationTokenAsync(createdStudent.Id);

            return created ? Ok() : BadRequest("New student could not be registered");
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
            var changed = await userService.ChangePasswordAsync(User.GetUserId(), passwordModel);

            return changed ? Ok() : BadRequest("Password could be not changed");
        }

        [SwaggerOperation(Summary = "Activates the account of the currently logged in user by actiavtion token")]
        [HttpPost("activate")]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> ActivateAccountByToken(string token)
        {
            var activated = await userService.ActivateUserByTokenAsync(User.GetUserId(), token);

            return activated ? Ok() : BadRequest("Account could be not activated");
        }

        [SwaggerOperation(Summary = "Sends a new activation token for the currently logged in user")]
        [HttpPost("newCode")]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> SendNewActivationToken()
        {
            var sent = await userService.SendNewActivationTokenAsync(User.GetUserId());

            return sent ? Ok() : BadRequest("New activation code could not be sent");
        }

        [SwaggerOperation(Summary = "Deactivates the account of the currently logged in user")]
        [HttpDelete]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> DeactivateAccount()
        {
            var deleted = await userService.DeactivateUserAsync(User.GetUserId());

            return deleted ? NoContent() : BadRequest("Account could be not deleted");
        }

        [SwaggerOperation(Summary = "Updates basic user information")]
        [HttpPut]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> UpdateGeneralInformation([FromBody] UpdatedUserDto model)
        {
            var updated = await userService.UpdateGeneralUserInfoAsync(User.GetUserId(), model);

            return updated ? NoContent() : BadRequest("User could be not updated");
        }

        [SwaggerOperation(Summary = "Retrieves basic information about the currently logged in user")]
        [HttpGet]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult<ShortUserDto>> GetGeneralUserInfo()
        {
            var user = await userService.GetGeneralUserInfoAsync(User.GetUserId());

            return Ok(user);
        }
    }
}