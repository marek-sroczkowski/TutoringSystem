using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.Action
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
                        if (!reservationRepository.ReservationExists(r => r.Id.Equals(reservationId.Value)))
                        {
                            context.Result = new NotFoundObjectResult(reservationId.Value);
                            return;
                        }
                    }
                }

                await next();
            }
        }
    }
}