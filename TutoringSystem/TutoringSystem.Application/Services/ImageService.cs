using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IUserRepository userRepository;

        public ImageService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<ProfileImageDto> GetProfileImageByUserId(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));

            return new ProfileImageDto { UserId = user.Id, ProfilePictureBase64 = user.ProfilePictureBase64 };
        }

        public async Task<bool> RemoveProfilePictureAsync(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));
            user.ProfilePictureBase64 = null;

            return await userRepository.UpdateUser(user);
        }

        public async Task<bool> SetProfileImageAsync(long userId, string imageBase64)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));
            user.ProfilePictureBase64 = imageBase64;

            return await userRepository.UpdateUser(user);
        }
    }
}