using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace App.Attributes
{
    public class ExceptionFilterAttribute : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilterAttribute> _logger;

        public ExceptionFilterAttribute(ILogger<ExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Error");

            context.Result = new RedirectToActionResult("Error", "Home", new
            {
                message = context.Exception.Message,
                messageType = "danger"
            });
        }
    }
}