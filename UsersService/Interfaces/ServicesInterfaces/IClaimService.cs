namespace UsersService.Interfaces.ServicesInterfaces
{
    public interface IClaimService
    {
        Guid GetUserIdFromTokenClaim();
    }
}
