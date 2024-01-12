using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace UsersServiceTests.Common
{
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public string UserEntiteId { get; set; } = null!;
    }

    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public const string AuthenticationScheme = "Test";

        public TestAuthHandler(
            IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) 
            : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();

            if (Context.Request.Headers.TryGetValue("userEntiteId", out var userId))
            {
                claims.Add(new Claim("userEntiteId" , userId!));
            }

            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
