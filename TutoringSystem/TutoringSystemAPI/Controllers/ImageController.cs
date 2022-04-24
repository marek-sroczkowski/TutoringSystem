using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Image;
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
        public async Task<ActionResult<ProfileImageDetailsDto>> GetProfileImage()
        {
            var picture = await imageService.GetProfileImageByUserId(User.GetUserId());

            return Ok(picture);
        }

        [SwaggerOperation(Summary = "Retrieves profile pictures of all students of the currently logged-in tutor")]
        [HttpGet("students")]
        [Authorize(Roles = "Tutor")]
        public async Task<ActionResult<IEnumerable<ProfileImageDetailsDto>>> GetStudentPhotos()
        {
            var pictures = await imageService.GetStudentPhotos(User.GetUserId());

            return Ok(pictures);
        }

        [SwaggerOperation(Summary = "Retrieves profile pictures of all tutors of the currently logged-in student")]
        [HttpGet("tutors")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<IEnumerable<ProfileImageDetailsDto>>> GetTutorPhotos()
        {
            var pictures = await imageService.GetTutorPhotos(User.GetUserId());

            return Ok(pictures);
        }

        [SwaggerOperation(Summary = "Sets profile picture for the currently logged in user")]
        [HttpPatch]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> SetProfileImage([FromBody] ProfileImageDto image)
        {
            var set = await imageService.SetProfileImageAsync(User.GetUserId(), image.ProfilePictureFirebaseUrl);

            return set ? NoContent() : BadRequest("Picture could be not set");
        }

        [SwaggerOperation(Summary = "Removes profile picture for the currently logged in user")]
        [HttpDelete]
        [Authorize(Roles = "Tutor,Student")]
        public async Task<ActionResult> RemoveProfileImage()
        {
            var deleted = await imageService.RemoveProfilePictureAsync(User.GetUserId());

            return deleted ? NoContent() : BadRequest("Picture could be not deleted");
        }
    }
}