using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters
{
    public class ValidateStudentExistenceAttribute : TypeFilterAttribute
    {
        public ValidateStudentExistenceAttribute() : base(typeof(ValidateStudentExistenceFilterImpl))
        {
        }

        private class ValidateStudentExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IStudentRepository studentRepository;

            public ValidateStudentExistenceFilterImpl(IStudentRepository studentRepository)
            {
                this.studentRepository = studentRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("studentId"))
                {
                    var studentId = context.ActionArguments["studentId"] as long?;
                    if (studentId.HasValue)
                    {
                        if ((await studentRepository.GetStudentByIdAsync(studentId.Value)) == null)
                        {
                            context.Result = new NotFoundObjectResult(studentId.Value);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
