using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StocksApp.Web.Filters.ActionFilters
{
    public class OnlyAjaxActionFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            var isAjax = request.Headers["x-requested-with"] == "XMLHttpRequest";
            if (!isAjax)
            {
                context.Result = new NotFoundResult();
            }
            await next();
        }
    }
}
