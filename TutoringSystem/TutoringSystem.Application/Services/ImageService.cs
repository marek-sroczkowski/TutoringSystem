using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.Image;
using TutoringSystem.Application.Services.Interfaces;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IUserRepository userRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly IMapper mapper;

        public ImageService(IUserRepository userRepository,
            IStudentRepository studentRepository,
            ITutorRepository tutorRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.studentRepository = studentRepository;
            this.tutorRepository = tutorRepository;
            this.mapper = mapper;
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

            return mapper.Map<IEnumerable<ProfileImageDetailsDto>>(students);
        }

        public async Task<IEnumerable<ProfileImageDetailsDto>> GetTutorPhotos(long studentId)
        {
            var student = await studentRepository.GetStudentAsync(s => s.Id.Equals(studentId));
            var tutors = tutorRepository.GetTutorsCollection(t => t.Students.Contains(student)).ToList();

            return mapper.Map<IEnumerable<ProfileImageDetailsDto>>(tutors);
        }

        public async Task<bool> RemoveProfilePictureAsync(long userId)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));
            user.ProfilePictureFirebaseUrl = null;

            return await userRepository.UpdateUserAsync(user);
        }

        public async Task<bool> SetProfileImageAsync(long userId, string firebaseUrl)
        {
            var user = await userRepository.GetUserAsync(u => u.Id.Equals(userId));
            user.ProfilePictureFirebaseUrl = firebaseUrl;

            return await userRepository.UpdateUserAsync(user);
        }
    }
}