using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.ContactDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.TypeFilters
{
    public class ValidateContactExistenceAttribute : TypeFilterAttribute
    {
        public ValidateContactExistenceAttribute() : base(typeof(ValidateContactExistenceFilterImpl))
        {
        }

        private class ValidateContactExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IContactRepository contactRepository;

            public ValidateContactExistenceFilterImpl(IContactRepository contactRepository)
            {
                this.contactRepository = contactRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("model"))
                {
                    var contact = context.ActionArguments["model"] as UpdatedContactDto;
                    if (contact != null)
                    {
                        if ((await contactRepository.GetContactAsync(c => c.Id.Equals(contact.Id))) == null)
                        {
                            context.Result = new NotFoundObjectResult(contact.Id);
                            return;
                        }
                    }
                }
                else if (context.ActionArguments.ContainsKey("contactId"))
                {
                    var contactId = context.ActionArguments["contactId"] as long?;
                    if (contactId.HasValue)
                    {
                        if ((await contactRepository.GetContactAsync(c => c.Id.Equals(contactId.Value))) == null)
                        {
                            context.Result = new NotFoundObjectResult(contactId.Value);
                            return;
                        }
                    }
                }

                await next();
            }
        }
    }
}
