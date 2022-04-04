using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [SwaggerOperation(Summary = "Generates a jwt token when logging in successfully")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResposneDto>> Authenticate([FromBody] AuthenticationDto authenticationModel)
        {
            var loginResult = await authenticationService.AuthenticateAsync(authenticationModel);

            return loginResult.Status == AuthenticationStatus.InvalidUsernameOrPassword
                ? BadRequest(loginResult)
                : Ok(loginResult);
        }
    }
}