using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.PhoneNumberDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.Action
{
    public class ValidatePhoneNumberExistenceAttribute : TypeFilterAttribute
    {
        public ValidatePhoneNumberExistenceAttribute() : base(typeof(ValidatePhoneNumberExistenceFilterImpl))
        {
        }

        private class ValidatePhoneNumberExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IPhoneNumberRepository phoneNumberRepository;

            public ValidatePhoneNumberExistenceFilterImpl(IPhoneNumberRepository phoneNumberRepository)
            {
                this.phoneNumberRepository = phoneNumberRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("model"))
                {
                    var phone = context.ActionArguments["model"] as UpdatedPhoneNumberDto;
                    if (phone != null)
                    {
                        if (!phoneNumberRepository.IsPhoneNumberExist(p => p.Id.Equals(phone.Id)))
                        {
                            context.Result = new NotFoundObjectResult(phone.Id);
                            return;
                        }
                    }
                }
                else if(context.ActionArguments.ContainsKey("phoneNumberId"))
                {
                    var phoneId = context.ActionArguments["phoneNumberId"] as long?;
                    if (phoneId.HasValue)
                    {
                        if (!phoneNumberRepository.IsPhoneNumberExist(p => p.Id.Equals(phoneId.Value)))
                        {
                            context.Result = new NotFoundObjectResult(phoneId.Value);
                            return;
                        }
                    }
                }

                await next();
            }
        }
    }
}