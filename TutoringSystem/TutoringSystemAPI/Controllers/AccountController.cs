using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Identity;
using TutoringSystem.Application.Service.Interfaces;
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
        [Authorize(Roles = "Admin,Tutor")]
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
        public async Task<ActionResult> Login([FromBody] LoginUserDto model)
        {
            var user = await userService.GetUserAsync(model);
            if (user == null)
                return BadRequest("Invalid username or password");

            var passwordVerificationResult = await userService.ValidatePasswordAsync(model);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
                return BadRequest("Invalid username of password!");

            var token = jwtProvider.GenerateJwtToken(user);
            Response.Headers.Add("Authorization", token);

            return Ok();
        }

        [SwaggerOperation(Summary = "Gets role of the currently logged user")]
        [HttpGet("role")]
        [Authorize(Roles = "Admin,Tutor,Student")]
        public async Task<ActionResult<Role>> GetUserRole()
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var role = await userService.GetUserRoleAsync(long.Parse(userId));

            return Ok(role);
        }

        [SwaggerOperation(Summary = "Changes password of the currently logged user")]
        [HttpPost("password")]
        [Authorize(Roles = "Admin,Tutor,Student")]
        public async Task<ActionResult> ChangePassword([FromBody] PasswordDto passwordModel)
        {
            var userId = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var changedErrors = await userService.ChangePasswordAsync(long.Parse(userId), passwordModel);

            if (changedErrors != null)
                return BadRequest(changedErrors);

            return Ok();
        }
    }
}
