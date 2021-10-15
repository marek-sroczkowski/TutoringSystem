using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.Application.Authorization
{
    public class PhoneNumberResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, PhoneNumberDetailsDto>
    {
        private readonly IContactService contactService;

        public PhoneNumberResourceOperationHandler(IContactService contactService)
        {
            this.contactService = contactService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, PhoneNumberDetailsDto resource)
        {
            if (requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.GetUserId();
            var contact = contactService.GetContactByUserAsync(userId);

            if (resource.Contact.Id == contact.Result.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
