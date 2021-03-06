using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AddressDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.Action
{
    public class ValidateAddressExistenceAttribute : TypeFilterAttribute
    {
        public ValidateAddressExistenceAttribute() : base(typeof(ValidateAddressExistenceFilterImpl))
        {
        }

        private class ValidateAddressExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IAddressRepository addressRepository;

            public ValidateAddressExistenceFilterImpl(IAddressRepository addressRepository)
            {
                this.addressRepository = addressRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("model"))
                {
                    var address = context.ActionArguments["model"] as UpdatedAddressDto;
                    if (address != null)
                    {
                        if (!addressRepository.IsAddressExist(a => a.Id.Equals(address.Id)))
                        {
                            context.Result = new NotFoundObjectResult(address.Id);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}