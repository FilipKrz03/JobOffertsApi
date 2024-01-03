using FirebaseAdmin.Auth;
using JobOffersApiCore.Exceptions;
using System.Security.Authentication;
using UsersService.Exceptions;

namespace UsersService.Middleware
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
            catch(FirebaseAuthException ex)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync(ex.Message);
            }
            catch(InvalidCredentialsException ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(ex.Message);
            }
            catch(InvalidRefreshTokenException ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(ex.Message);
            }
            catch(InvalidAccesTokenException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
            catch(ResourceAlreadyExistException ex)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync(ex.Message);
            }
            catch(ResourceNotFoundException ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Error occured on server side");
            }
        }
    }
}
