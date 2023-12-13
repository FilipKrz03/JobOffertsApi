namespace JobOffersService.Interfaces
{
    public interface IProcessedOfferService
    {
        Task HandleProcessedOffer(string body);
    }
}
