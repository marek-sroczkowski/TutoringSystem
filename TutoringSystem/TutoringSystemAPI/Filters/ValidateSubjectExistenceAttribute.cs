using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.SubjectDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters
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
                        if ((await subjectRepository.GetSubjectAsync(s => s.Id.Equals(subjectId.Value))) == null)
                        {
                            context.Result = new NotFoundObjectResult(subjectId.Value);
                            return;
                        }
                    }
                }
                else if (context.ActionArguments.ContainsKey("model"))
                {
                    var subject = context.ActionArguments["model"] as UpdatedSubjectDto;
                    if (subject != null)
                    {
                        if ((await subjectRepository.GetSubjectAsync(s => s.Id.Equals(subject.Id))) == null)
                        {
                            context.Result = new NotFoundObjectResult(subject.Id);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
