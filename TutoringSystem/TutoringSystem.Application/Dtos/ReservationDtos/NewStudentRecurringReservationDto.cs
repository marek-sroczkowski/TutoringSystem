using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Dtos.ReservationDtos
{
    public class NewStudentRecurringReservationDto : NewStudentSingleReservationDto, IMap
    {
        public ReservationFrequency Frequency { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<NewStudentRecurringReservationDto, RecurringReservation>();
        }
    }
}
