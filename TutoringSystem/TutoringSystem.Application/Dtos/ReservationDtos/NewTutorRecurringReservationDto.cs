using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class NewTutorRecurringReservationDto : NewTutorSingleReservationDto, IMap
    {
        public ReservationFrequency Frequency { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<NewTutorRecurringReservationDto, RecurringReservation>();
        }
    }
}
