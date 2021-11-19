using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IImageService
    {
        Task<ProfileImageDetailsDto> GetProfileImageByUserId(long userId);
        Task<bool> RemoveProfilePictureAsync(long userId);
        Task<bool> SetProfileImageAsync(long userId, string imageBase64);
    }
}
