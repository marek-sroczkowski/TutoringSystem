using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Authentication;
using TutoringSystem.Application.Dtos.Enums;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IRefreshTokenService refreshTokenService;

        public AuthenticationController(IAuthenticationService authenticationService, IRefreshTokenService refreshTokenService)
        {
            this.authenticationService = authenticationService;
            this.refreshTokenService = refreshTokenService;
        }

        [SwaggerOperation(Summary = "Generates a jwt token when logging in successfully")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AuthenticationResposneDto>> Authenticate([FromBody] AuthenticationDto authenticationModel)
        {
            string ip = Request.Headers.ContainsKey("X-Forwarded-For") ? Request.Headers["X-Forwarded-For"] : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var loginResult = await authenticationService.AuthenticateAsync(authenticationModel, ip);

            return loginResult.Status == AuthenticationStatus.InvalidUsernameOrPassword
                ? BadRequest(loginResult)
                : Ok(loginResult);
        }

        [SwaggerOperation(Summary = "Generates a new jwt token based on the refresh token and the device ID")]
        [HttpPost("refresh/jwt")]
        public async Task<ActionResult<JwtTokenDto>> RefreshJwt(JwtRefreshRequestDto jwtRefreshRequest)
        {
            var token = await refreshTokenService.GenerateRefreshedJwtTokenAsync(User.GetUserId(), jwtRefreshRequest);

            return Ok(token);
        }

        [SwaggerOperation(Summary = "Generates a new jwt token based on the refresh token and the device ID")]
        [HttpPost("refresh/token")]
        public async Task<ActionResult<RefreshTokenDto>> GenerateRefreshToken(JwtRefreshRequestDto refreshTokenRequest)
        {
            string ip = Request.Headers.ContainsKey("X-Forwarded-For") ? Request.Headers["X-Forwarded-For"] : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var token = await refreshTokenService.AddRefreshToken(User.GetUserId(), refreshTokenRequest.DeviceIdentificator, ip);

            return Ok(token);
        }
    }
}