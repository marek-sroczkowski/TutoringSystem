using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AvailabilityDtos;
using TutoringSystem.Application.Extensions;

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

            var tutorId = context.User.GetUserId();

            if (resource.Tutor.Id == tutorId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
