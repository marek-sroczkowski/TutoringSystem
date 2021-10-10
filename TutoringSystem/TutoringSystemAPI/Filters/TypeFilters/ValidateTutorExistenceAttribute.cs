using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.TypeFilters
{
    public class ValidateTutorExistenceAttribute : TypeFilterAttribute
    {
        public ValidateTutorExistenceAttribute() : base(typeof(ValidateTutorExistenceFilterImpl))
        {
        }

        private class ValidateTutorExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly ITutorRepository tutorRepository;

            public ValidateTutorExistenceFilterImpl(ITutorRepository tutorRepository)
            {
                this.tutorRepository = tutorRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("tutorId"))
                {
                    var tutorId = context.ActionArguments["tutorId"] as long?;
                    if (tutorId.HasValue)
                    {
                        if ((await tutorRepository.GetTutorAsync(t => t.Id.Equals(tutorId.Value))) == null)
                        {
                            context.Result = new NotFoundObjectResult(tutorId.Value);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
