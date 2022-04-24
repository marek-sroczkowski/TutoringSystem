using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Token;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/pushNotification/token")]
    [ApiController]
    [Authorize]
    public class PushNotificationTokenController : ControllerBase
    {
        private readonly IPushNotificationTokenService tokenService;

        public PushNotificationTokenController(IPushNotificationTokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [SwaggerOperation(Summary = "Updates a firebase push notification token")]
        [HttpPut]
        [Authorize(Roles = "Tutor, Student")]
        public async Task<ActionResult> UpdatePushNotificationToken([FromBody] PushNotificationTokenDto token)
        {
            var updated = await tokenService.PutTokenAsync(User.GetUserId(), token.RegistrationToken);

            return updated ? NoContent() : BadRequest("Token could be not updated");
        }
    }
}