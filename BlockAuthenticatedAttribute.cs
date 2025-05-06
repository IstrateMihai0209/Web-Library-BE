using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineLibrary;

public class BlockAuthenticatedAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new BadRequestObjectResult(new
            {
                Error = "You are already authenticated!"
            });
        }
        base.OnActionExecuting(context);
    }
}