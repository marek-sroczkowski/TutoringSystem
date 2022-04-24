using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Subject;

namespace TutoringSystem.Application.Authorization
{
    class SubjectResourceOperationHandler :  AuthorizationHandler<ResourceOperationRequirement, SubjectDetailsDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, SubjectDetailsDto resource)
        {
            if (requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }

            var tutorId = context.User.GetUserId();

            if (resource.TutorId == tutorId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
