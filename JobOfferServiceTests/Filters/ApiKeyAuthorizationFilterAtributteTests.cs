using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using JobOffersService.Filters;
using FluentAssertions;

namespace JobOfferServiceTests.Filters
{
    public class ApiKeyAuthorizationFilterAtributteTests
    {

        private readonly AuthorizationFilterContext _context;
        private readonly ApiKeyAuthorizationFilterAtributte _filter;

        public ApiKeyAuthorizationFilterAtributteTests()
        {
            _context = new(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                Array.Empty<IFilterMetadata>()
                );
            _filter = new();
        }

        [Fact]
        public void Filter_Should_ReturnUnautorizedResult_WhenApiKeyNotProvided()
        {
           _filter.OnAuthorization(_context);

            _context.Result.Should()
              .BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void Filter_Should_ReturnUnauthorizedResult_WhenInvalidApiKeyProvided()
        {
            Environment.SetEnvironmentVariable("ApiKey", "VALID_API_KEY");

            _context.HttpContext.Request.Headers["ApiKey"] = "INVALID_API_KEY";

            _filter.OnAuthorization(_context);

            _context.Result.Should()
               .BeOfType<UnauthorizedResult>();
        }

        [Fact]
        public void Filter_ShouldNot_ReturnUnautorizedResult_WhenValidApiKeyProvided()
        {
            string validApiKey = "VALID_API_KEY";

            Environment.SetEnvironmentVariable("ApiKey", validApiKey);

            _context.HttpContext.Request.Headers["ApiKey"] = validApiKey;

            _filter.OnAuthorization(_context);

            _context.Result.Should()
                .BeNull();
        }
    }
}
