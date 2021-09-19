using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.SubjectDtos;

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

            var tutorId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.Tutor.Id == long.Parse(tutorId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
