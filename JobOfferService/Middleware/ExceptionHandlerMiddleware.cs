using JobOffersApiCore.Exceptions;

namespace JobOffersService.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {

        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (ResourceNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Error occured on server side");
            }
        }
    }
}
