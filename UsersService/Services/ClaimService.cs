using UsersService.Exceptions;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Services
{

    public class ClaimService : IClaimService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserIdFromTokenClaim()
        {
            string? result =
                _httpContextAccessor?.HttpContext?.User?.FindFirst("userEntiteId")?.Value;

            if (result == null)
            {
                throw new InvalidAccesTokenException("Invalid access token");
            }

            Guid userId = Guid.Parse(result);

            return userId;
        }
    }
}
