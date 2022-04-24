using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Availability;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.Action
{
    public class ValidateAvailabilityExistenceAttribute : TypeFilterAttribute
    {
        public ValidateAvailabilityExistenceAttribute() : base(typeof(ValidateAvailabilityExistenceFilterImpl))
        {
        }

        private class ValidateAvailabilityExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IAvailabilityRepository availabilityRepository;

            public ValidateAvailabilityExistenceFilterImpl(IAvailabilityRepository availabilityRepository)
            {
                this.availabilityRepository = availabilityRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("availabilityId"))
                {
                    var availabilityId = context.ActionArguments["availabilityId"] as long?;
                    if (availabilityId.HasValue)
                    {
                        if (!availabilityRepository.AvailabilityExists(a => a.Id.Equals(availabilityId.Value)))
                        {
                            context.Result = new NotFoundObjectResult(availabilityId.Value);
                            return;
                        }
                    }
                }
                else if (context.ActionArguments.ContainsKey("model"))
                {
                    var availability = context.ActionArguments["model"] as UpdatedAvailabilityDto;
                    if (availability != null)
                    {
                        if (!availabilityRepository.AvailabilityExists(a => a.Id.Equals(availability.Id)))
                        {
                            context.Result = new NotFoundObjectResult(availability.Id);
                            return;
                        }
                    }
                }

                await next();
            }
        }
    }
}