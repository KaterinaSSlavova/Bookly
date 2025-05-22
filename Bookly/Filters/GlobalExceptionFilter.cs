using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bookly.Filters
{
    public class GlobalExceptionFilter: IExceptionFilter
    {
       private readonly ILogger<GlobalExceptionFilter> _logger;
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            context.Result = new ViewResult { ViewName = "Error" };
            context.ExceptionHandled = true;    
        }
    }
}
