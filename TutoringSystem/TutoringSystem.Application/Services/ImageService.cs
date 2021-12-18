using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AccountDtos;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IUserRepository userRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITutorRepository tutorRepository;

        public ImageService(IUserRepository userRepository,
            IStudentRepository studentRepository,
            ITutorRepository tutorRepository)
        {
            this.userRepository = userRepository;
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
        }

        public async Task<ProfileImageDetailsDto> GetProfileImageByUserId(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));

            return new ProfileImageDetailsDto { UserId = user.Id, ProfilePictureFirebaseUrl = user.ProfilePictureFirebaseUrl };
        }

        public async Task<IEnumerable<ProfileImageDetailsDto>> GetStudentPhotos(long tutorId)
        {
            var tutor = await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId));
            var students = await studentRepository.GetStudentsCollectionAsync(s => s.Tutors.Contains(tutor));

            return students.Select(s => new ProfileImageDetailsDto(s));
        }

        public async Task<IEnumerable<ProfileImageDetailsDto>> GetTutorPhotos(long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            var tutors = await tutorRepository.GetTutorsCollectionAsync(t => t.Students.Contains(student));

            return tutors.Select(t => new ProfileImageDetailsDto(t));
        }

        public async Task<bool> RemoveProfilePictureAsync(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));
            user.ProfilePictureFirebaseUrl = null;

            return await userRepository.UpdateUser(user);
        }

        public async Task<bool> SetProfileImageAsync(long userId, string imageBase64)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));
            user.ProfilePictureFirebaseUrl = imageBase64;

            return await userRepository.UpdateUser(user);
        }
    }
}