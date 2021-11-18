using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.API.Controllers
{
    [Route("api/image")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;

        public ImageController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [SwaggerOperation(Summary = "Retrieves profile picture for the currently logged in user")]
        [HttpGet]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult<ProfileImageDto>> GetProfileImage()
        {
            var picture = await imageService.GetProfileImageByUserId(User.GetUserId());

            return Ok(picture);
        }

        [SwaggerOperation(Summary = "Sets profile picture for the currently logged in user")]
        [HttpPatch]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> SetProfileImage([FromBody] string imageBase64)
        {
            var set = await imageService.SetProfileImageAsync(User.GetUserId(), imageBase64);
            if (!set)
                return BadRequest("Picture could be not set");

            return NoContent();
        }

        [SwaggerOperation(Summary = "Removes profile picture for the currently logged in user")]
        [HttpDelete]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> RemoveProfileImage()
        {
            var deleted = await imageService.RemoveProfilePictureAsync(User.GetUserId());
            if (!deleted)
                return BadRequest("Picture could be not deleted");

            return NoContent();
        }
    }
}
