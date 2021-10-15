using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.Application.Authorization
{
    public class ContactResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, UpdatedContactDto>
    {
        private readonly IContactService contactService;

        public ContactResourceOperationHandler(IContactService contactService)
        {
            this.contactService = contactService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, UpdatedContactDto resource)
        {
            if (requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.GetUserId();
            var contact = contactService.GetContactByUserAsync(userId);

            if (resource.Id == contact.Result.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
