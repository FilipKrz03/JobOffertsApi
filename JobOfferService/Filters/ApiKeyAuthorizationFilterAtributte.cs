using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobOffersService.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class , AllowMultiple = true)]
    public class ApiKeyAuthorizationFilterAtributte : Attribute , IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");

            var request = context.HttpContext.Request;
            var providedApiKey = request.Headers["ApiKey"].FirstOrDefault();

            if(providedApiKey == null || providedApiKey != apiKey)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
