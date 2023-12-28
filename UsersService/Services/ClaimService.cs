using UsersService.Interfaces;
using UsersService.Exceptions;

namespace UsersService.Services
{

    public class ClaimService : IClaimService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserIdentityIdFromTokenClaim()
        {
            string? result =
                _httpContextAccessor?.HttpContext?.User?.FindFirst("user_id")?.Value;

            if (result == null)
            {
                throw new InvalidAccesTokenException("Invalid access token");
            }

            return result;
        }
    }
}
