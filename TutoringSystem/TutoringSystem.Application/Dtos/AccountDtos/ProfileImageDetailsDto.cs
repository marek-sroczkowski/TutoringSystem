using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.AccountDtos
{
    public class ProfileImageDetailsDto
    {
        public long UserId { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }

        public ProfileImageDetailsDto()
        {
        }

        public ProfileImageDetailsDto(User user)
        {
            UserId = user.Id;
            ProfilePictureFirebaseUrl = user.ProfilePictureFirebaseUrl;
        }
    }
}