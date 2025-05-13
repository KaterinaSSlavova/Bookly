using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Bookly.Filters
{
    public class BookExceptionFilter: IExceptionFilter
    {
       private readonly ILogger<BookExceptionFilter> _logger;
        public BookExceptionFilter(ILogger<BookExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Book-related unhandled exception occurred.");
            context.Result = new ViewResult { ViewName = "Error" };
            context.ExceptionHandled = true;    
        }
    }
}
