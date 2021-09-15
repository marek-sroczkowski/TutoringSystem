using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;

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

            var tutorId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (resource.Tutor.Id == long.Parse(tutorId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
