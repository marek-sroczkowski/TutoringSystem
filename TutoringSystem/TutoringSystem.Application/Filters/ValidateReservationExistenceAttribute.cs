using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ReservationDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Filters
{
    public class ValidateReservationExistenceAttribute : TypeFilterAttribute
    {
        public ValidateReservationExistenceAttribute() : base(typeof(ValidateReservationExistenceFilterImpl))
        {
        }

        private class ValidateReservationExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IReservationRepository reservationRepository;

            public ValidateReservationExistenceFilterImpl(IReservationRepository reservationRepository)
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
                        if ((await reservationRepository.GetReservationByIdAsync(reservationId.Value)) == null)
                        {
                            context.Result = new NotFoundObjectResult(reservationId.Value);
                            return;
                        }
                    }
                }
                else if (context.ActionArguments.ContainsKey("model"))
                {
                    var reservation = context.ActionArguments["model"] as UpdatedReservationDto;
                    if (reservation != null)
                    {
                        if ((await reservationRepository.GetReservationByIdAsync(reservation.Id)) == null)
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
