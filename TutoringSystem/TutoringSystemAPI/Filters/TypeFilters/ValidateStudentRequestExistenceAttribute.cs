using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.TypeFilters
{
    public class ValidateStudentRequestExistenceAttribute : TypeFilterAttribute
    {
        public ValidateStudentRequestExistenceAttribute() : base(typeof(ValidateStudentRequestExistenceFilterImpl))
        {
        }

        private class ValidateStudentRequestExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IStudentTutorRequestRepository requestRepository;

            public ValidateStudentRequestExistenceFilterImpl(IStudentTutorRequestRepository requestRepository)
            {
                this.requestRepository = requestRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("requestId"))
                {
                    var requestId = context.ActionArguments["requestId"] as long?;
                    if (requestId.HasValue)
                    {
                        if ((await requestRepository.GetRequestAsync(r => r.Id.Equals(requestId.Value))) == null)
                        {
                            context.Result = new NotFoundObjectResult(requestId.Value);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}