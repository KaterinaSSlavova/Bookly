using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bookly.Filters
{
    public class FilterLoggedUsers: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Session.GetString("Username") == null)
            {
                context.Result = new RedirectToActionResult("LogIn", "User", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
