using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Filters
{
    public class ValidateSubjectExistenceAttribute : TypeFilterAttribute
    {
        public ValidateSubjectExistenceAttribute() : base(typeof(ValidateSubjectExistenceFilterImpl))
        {
        }

        private class ValidateSubjectExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly ISubjectRepository subjectRepository;

            public ValidateSubjectExistenceFilterImpl(ISubjectRepository subjectRepository)
            {
                this.subjectRepository = subjectRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("subjectId"))
                {
                    var subjectId = context.ActionArguments["subjectId"] as long?;
                    if (subjectId.HasValue)
                    {
                        if ((await subjectRepository.GetSubjectByIdAsync(subjectId.Value)) == null)
                        {
                            context.Result = new NotFoundObjectResult(subjectId.Value);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
