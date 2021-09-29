using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AvailabilityDtos;

namespace TutoringSystem.Application.Authorization
{
    public class AvailabilityResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, AvailabilityDetailsDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, AvailabilityDetailsDto resource)
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
