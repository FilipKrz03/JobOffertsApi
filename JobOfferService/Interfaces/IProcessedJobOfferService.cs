namespace JobOffersService.Interfaces
{
    public interface IProcessedOfferService
    {
        Task HandleProcessedOfferAsync(string body);
    }
}
