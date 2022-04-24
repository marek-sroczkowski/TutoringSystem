using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Availability;

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

            if (resource.TutorId == tutorId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
