using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StocksApp.ServiceContracts.DTOs;
using StocksApp.Services.Helpers;
using StocksApp.Web.Controllers;
using StocksApp.Web.Models;

namespace StocksApp.Web.Filters.ActionFilters
{
    public class CreateOrderActionFactororyAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new CreateOrderActionFilter();
        }
    }
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is TradeController tradeController)
            {
                var request = context.ActionArguments["request"] as IOrderRequest;
                if (request != null)
                {
                    request.DateAndTimeOfOrder = DateTime.UtcNow;
                    tradeController.ModelState.Clear();

                    tradeController.TryValidateModel(request);

                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage);
                        var stockTrade = new StockTrade()
                        {
                            StockName = request.StockName,
                            StockSymbol = request.StockSymbol,
                            Price = request.Price,
                            Quantity = request.Quantity
                        };

                        context.Result = tradeController.View("Index", stockTrade);
                    }
                    else
                        await next();
                }
                else
                    await next();
            }else
                await next();
        }
    }
}
