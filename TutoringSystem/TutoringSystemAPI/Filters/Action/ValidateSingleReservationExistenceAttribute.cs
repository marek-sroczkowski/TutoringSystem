using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.Action
{
    public class ValidateSingleReservationExistenceAttribute : TypeFilterAttribute
    {
        public ValidateSingleReservationExistenceAttribute() : base(typeof(ValidateReservationExistenceFilterImpl))
        {
        }

        private class ValidateReservationExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly ISingleReservationRepository reservationRepository;

            public ValidateReservationExistenceFilterImpl(ISingleReservationRepository reservationRepository)
            {
                this.reservationRepository = reservationRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("reservationId"))
                {
                    var reservationId = context.ActionArguments["reservationId"] as long?;
                    if (reservationId.HasValue)
                    {
                        if (!reservationRepository.ReservationExists(r => r.Id.Equals(reservationId.Value)))
                        {
                            context.Result = new NotFoundObjectResult(reservationId.Value);
                            return;
                        }
                    }
                }
                else if (context.ActionArguments.ContainsKey("model"))
                {
                    var reservation = context.ActionArguments["model"] as UpdatedTutorReservationDto;
                    if (reservation != null)
                    {
                        if (!reservationRepository.ReservationExists(r => r.Id.Equals(reservation.Id)))
                        {
                            context.Result = new NotFoundObjectResult(reservation.Id);
                            return;
                        }
                    }
                }

                await next();
            }
        }
    }
}