using AutoMapper;
using TutoringSystem.Application.Mapping;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;

namespace TutoringSystem.Application.Models.Dtos.Reservation
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
