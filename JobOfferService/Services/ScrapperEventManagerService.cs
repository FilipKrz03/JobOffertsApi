using JobOffersApiCore.Interfaces;
using JobOfferService.Props;
using JobOffersService.Interfaces;
using JobOffersService.Services;

namespace JobOfferService.Services
{
    public class ScrapperEventManagerService : BackgroundService
    {

        private readonly IRabbitMessageProducer _scrapperMessageProducer;
        private readonly IServiceProvider _serviceProvider;

        public ScrapperEventManagerService(IRabbitMessageProducer scrapperMessageProducer , 
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _scrapperMessageProducer = scrapperMessageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IOfferRepository jobOfferRepository = 
                        scope.ServiceProvider.GetService<IOfferRepository>()!;

                    bool isDatabaseInitialized = await jobOfferRepository.IsDatabaseInitalizedAsync();

                    string routingKey = isDatabaseInitialized switch
                    {
                        true => RabbitMQOffersScraperProps.OFFERS_UPDATE_ROUTING_KEY,
                        false => RabbitMQOffersScraperProps.OFFERS_CREATE_ROUTING_KEY
                    };

                    _scrapperMessageProducer.SendMessage
                        (RabbitMQOffersScraperProps.OFFERS_SCRAPER_EXCHANGE, routingKey);
                }

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }
    }
}
