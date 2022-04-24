using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Models.Dtos.Contact;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.Action
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
                        if (!contactRepository.IsContactExist(c => c.Id.Equals(contact.Id)))
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
                        if (!contactRepository.IsContactExist(c => c.Id.Equals(contactId.Value)))
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