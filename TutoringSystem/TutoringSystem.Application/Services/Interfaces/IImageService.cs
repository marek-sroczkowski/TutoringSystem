using System.Collections.Generic;
using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Image;

namespace TutoringSystem.Application.Services.Interfaces
{
    public interface IImageService
    {
        Task<ProfileImageDetailsDto> GetProfileImageByUserId(long userId);
        Task<IEnumerable<ProfileImageDetailsDto>> GetStudentPhotos(long tutorId);
        Task<IEnumerable<ProfileImageDetailsDto>> GetTutorPhotos(long studentId);
        Task<bool> RemoveProfilePictureAsync(long userId);
        Task<bool> SetProfileImageAsync(long userId, string firebaseUrl);
    }
}