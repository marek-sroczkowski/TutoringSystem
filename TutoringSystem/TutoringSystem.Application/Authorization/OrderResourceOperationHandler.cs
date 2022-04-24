using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Models.Dtos.Order;

namespace TutoringSystem.Application.Authorization
{
    public class OrderResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, OrderDetailsDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, OrderDetailsDto resource)
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
