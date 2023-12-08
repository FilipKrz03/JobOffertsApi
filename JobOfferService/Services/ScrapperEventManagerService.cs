using JobOffersApiCore.Interfaces;
using JobOfferService.Props;

namespace JobOfferService.Services
{
    public class ScrapperEventManagerService : BackgroundService
    {

        IRabbitMessageProducer _scrapperMessageProducer;

        public ScrapperEventManagerService(IRabbitMessageProducer scrapperMessageProducer)
        {
            _scrapperMessageProducer = scrapperMessageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _scrapperMessageProducer.SendMessage
                    (RabbitMQOffersScraperEventProps.OFFERS_SCRAPER_EXCHANGE, RabbitMQOffersScraperEventProps.OFFERS_CREATE_ROUTING_KEY);

                await Task.Delay(TimeSpan.FromMinutes(60));
            }
        }
    }
}
