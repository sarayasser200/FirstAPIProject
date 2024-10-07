using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace FirstAPIProject.Filters
{
    public class logActivityFilter : IActionFilter,IAsyncActionFilter
    {
        private readonly ILogger<logActivityFilter> _logger;

        public logActivityFilter(ILogger<logActivityFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Executing action {context.ActionDescriptor.DisplayName} on controllor {context.Controller} with arguments {JsonSerializer.Serialize( context.ActionArguments)}");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} finished  executed on controller {context.Controller}");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _logger.LogInformation($"(Async) Executing action {context.ActionDescriptor.DisplayName} on controllor {context.Controller} with arguments {JsonSerializer.Serialize(context.ActionArguments)}");
            await next();
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} finished  executed on controller {context.Controller}");
            
        }
    }
}
