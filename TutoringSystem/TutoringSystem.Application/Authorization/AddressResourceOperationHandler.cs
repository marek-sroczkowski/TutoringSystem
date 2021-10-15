using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AddressDtos;
using TutoringSystem.Application.Extensions;
using TutoringSystem.Application.Services.Interfaces;

namespace TutoringSystem.Application.Authorization
{
    public class AddressResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, UpdatedAddressDto>
    {
        private readonly IAddressService addressService;

        public AddressResourceOperationHandler(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, UpdatedAddressDto resource)
        {
            if (requirement.OperationType == OperationType.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.GetUserId();
            var address = addressService.GetAddressByUserAsync(userId);

            if (resource.Id == address.Result.Id)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
