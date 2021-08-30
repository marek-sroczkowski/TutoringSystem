using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.Application.Filters
{
    public class ValidateOrderExistenceAttribute : TypeFilterAttribute
    {
        public ValidateOrderExistenceAttribute() : base(typeof(ValidateOrderExistenceFilterImpl))
        {
        }

        private class ValidateOrderExistenceFilterImpl : IAsyncActionFilter
        {
            private readonly IAdditionalOrderRepository additionalOrderRepository;

            public ValidateOrderExistenceFilterImpl(IAdditionalOrderRepository additionalOrderRepository)
            {
                this.additionalOrderRepository = additionalOrderRepository;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("orderId"))
                {
                    var orderId = context.ActionArguments["orderId"] as long?;
                    if (orderId.HasValue)
                    {
                        if ((await additionalOrderRepository.GetAdditionalOrderByIdAsync(orderId.Value)) == null)
                        {
                            context.Result = new NotFoundObjectResult(orderId.Value);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}
