using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;

namespace TutoringSystem.Application.Dtos.Image
{
    public class ProfileImageDetailsDto : IMap
    {
        public long UserId { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, ProfileImageDetailsDto>()
                .ForMember(dest => dest.UserId, map => map.MapFrom(src => src.Id));

            profile.CreateMap<Student, ProfileImageDetailsDto>()
                .ForMember(dest => dest.UserId, map => map.MapFrom(src => src.Id));

            profile.CreateMap<Tutor, ProfileImageDetailsDto>()
                .ForMember(dest => dest.UserId, map => map.MapFrom(src => src.Id));
        }
    }
}