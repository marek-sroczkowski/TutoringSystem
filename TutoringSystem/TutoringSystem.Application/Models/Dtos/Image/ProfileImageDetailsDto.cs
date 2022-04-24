using AutoMapper;
using TutoringSystem.Application.Mapping;

namespace TutoringSystem.Application.Models.Dtos.Image
{
    public class ProfileImageDetailsDto : IMap
    {
        public long UserId { get; set; }
        public string ProfilePictureFirebaseUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.User, ProfileImageDetailsDto>()
                .ForMember(dest => dest.UserId, map => map.MapFrom(src => src.Id));

            profile.CreateMap<Domain.Entities.Student, ProfileImageDetailsDto>()
                .ForMember(dest => dest.UserId, map => map.MapFrom(src => src.Id));

            profile.CreateMap<Domain.Entities.Tutor, ProfileImageDetailsDto>()
                .ForMember(dest => dest.UserId, map => map.MapFrom(src => src.Id));
        }
    }
}