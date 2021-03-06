using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using TutoringSystem.Application.BackgroundServices;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Domain.Entities;
using TutoringSystem.Domain.Entities.Enums;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.ScheduleTasks
{
    class RecurringReservationSynchronization : ScheduledProcessor
    {
        public RecurringReservationSynchronization(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        protected override string Schedule => "55 23 * * *";

        public override async Task ProcessInScopeAsync(IServiceProvider scopeServiceProvider)
        {
            IRepeatedReservationRepository reservationRepository = scopeServiceProvider.GetRequiredService<IRepeatedReservationRepository>();
            var tomorrowDate = DateTime.Now.ToLocal().AddDays(1).Date;
            var now = DateTime.Now.ToLocal().Date;
            var reservations = reservationRepository.GetReservationsCollection(r => r.NextAddedDate.Date.Equals(tomorrowDate) || r.NextAddedDate.Date <= now).ToList();
            
            for (int i = 0; i < reservations.Count; i++)
            {
                switch (reservations[i].Frequency)
                {
                    case ReservationFrequency.Weekly:
                        await SynchronizeWeeklyReservationAsync(reservations[i], reservationRepository);
                        break;
                    case ReservationFrequency.OnceTwoWeeks:
                        await SynchronizeOnceTwoWeeksReservationAsync(reservations[i], reservationRepository);
                        break;
                    case ReservationFrequency.Monthly:
                        await SynchronizeMonthlyReservationAsync(reservations[i], reservationRepository);
                        break;
                }
            }

            await Task.Run(() =>
            {
                return Task.CompletedTask;
            });
        }

        private static async Task SynchronizeWeeklyReservationAsync(RepeatedReservation reservation, IRepeatedReservationRepository reservationRepository)
        {
            if (reservation.LastAddedDate.Date > DateTime.Now.ToLocal().AddDays(-6).Date)
                return;

            while (reservation.LastAddedDate.Date <= DateTime.Now.ToLocal().AddDays(-6).Date)
            {
                var recurringReservation = reservation.Reservations.Last();
                reservation.Reservations.Add(new RecurringReservation(recurringReservation)
                {
                    StartTime = recurringReservation.StartTime.AddDays(7)
                });
                reservation.LastAddedDate = recurringReservation.StartTime.AddDays(7);
                reservation.NextAddedDate = reservation.NextAddedDate.AddDays(7);
            }

            await reservationRepository.UpdateReservationAsync(reservation);
        }

        private async Task SynchronizeOnceTwoWeeksReservationAsync(RepeatedReservation reservation, IRepeatedReservationRepository reservationRepository)
        {
            if (reservation.LastAddedDate.Date > DateTime.Now.ToLocal().AddDays(-13).Date)
                return;

            while (reservation.LastAddedDate.Date <= DateTime.Now.ToLocal().AddDays(-13).Date)
            {
                var recurringReservation = reservation.Reservations.Last();
                reservation.Reservations.Add(new RecurringReservation(recurringReservation)
                {
                    StartTime = recurringReservation.StartTime.AddDays(14)
                });
                reservation.LastAddedDate = recurringReservation.StartTime.AddDays(14);
                reservation.NextAddedDate = reservation.NextAddedDate.AddDays(14);
            }

            await reservationRepository.UpdateReservationAsync(reservation);
        }

        private async Task SynchronizeMonthlyReservationAsync(RepeatedReservation reservation, IRepeatedReservationRepository reservationRepository)
        {
            if (reservation.LastAddedDate.Date > DateTime.Now.ToLocal().AddDays(-27).Date)
                return;

            while (reservation.LastAddedDate.Date <= DateTime.Now.ToLocal().AddDays(-27).Date)
            {
                var recurringReservation = reservation.Reservations.Last();
                reservation.Reservations.Add(new RecurringReservation(recurringReservation)
                {
                    StartTime = recurringReservation.StartTime.AddDays(28)
                });
                reservation.LastAddedDate = recurringReservation.StartTime.AddDays(28);
                reservation.NextAddedDate = reservation.NextAddedDate.AddDays(28);
            }

            await reservationRepository.UpdateReservationAsync(reservation);
        }
    }
}