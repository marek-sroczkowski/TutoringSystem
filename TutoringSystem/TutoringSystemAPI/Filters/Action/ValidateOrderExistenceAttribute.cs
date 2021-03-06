using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using TutoringSystem.Application.Dtos.AdditionalOrderDtos;
using TutoringSystem.Domain.Repositories;

namespace TutoringSystem.API.Filters.Action
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
                        if (!additionalOrderRepository.IsOrderExist(o => o.Id.Equals(orderId.Value)))
                        {
                            context.Result = new NotFoundObjectResult(orderId.Value);
                            return;
                        }
                    }
                }
                else if(context.ActionArguments.ContainsKey("model"))
                {
                    var order = context.ActionArguments["model"] as UpdatedOrderDto;
                    if(order != null)
                    {
                        if (!additionalOrderRepository.IsOrderExist(o => o.Id.Equals(order.Id)))
                        {
                            context.Result = new NotFoundObjectResult(order.Id);
                            return;
                        }
                    }
                }
                await next();
            }
        }
    }
}