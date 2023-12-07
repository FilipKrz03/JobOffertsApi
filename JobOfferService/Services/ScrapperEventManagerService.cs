using JobOfferService.Interfaces;

namespace JobOfferService.Services
{
    public class ScrapperEventManagerService : BackgroundService
    {

        IScrapperMessageProducer _scrapperMessageProducer;

        public ScrapperEventManagerService(IScrapperMessageProducer scrapperMessageProducer)
        {
            _scrapperMessageProducer = scrapperMessageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _scrapperMessageProducer.SendCreateOffersMessage();

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}
