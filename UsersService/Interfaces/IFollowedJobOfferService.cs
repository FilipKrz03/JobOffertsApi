namespace UsersService.Interfaces
{
    public interface IFollowedJobOfferService
    {
        Task AddFolowedJobOffer(Guid offerId);
        Task DeleteFollowedJobOffer(Guid followedJobOfferId);
    }
}
