using JobOffersApiCore.Interfaces;
using JobOfferService.Props;
using JobOffersService.Interfaces;

namespace JobOfferService.Services
{
    public class ScrapperEventManagerService : BackgroundService
    {

        private readonly IRabbitMessageProducer _scrapperMessageProducer;
        private readonly IJobOfferRepository _jobOfferRepository;

        public ScrapperEventManagerService(IRabbitMessageProducer scrapperMessageProducer , 
            IJobOfferRepository jobOfferRepository)
        {
            _jobOfferRepository = jobOfferRepository;
            _scrapperMessageProducer = scrapperMessageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                bool isDatabaseInitialized = await _jobOfferRepository.IsDatabaseInitalized();

                string routingKey = isDatabaseInitialized switch
                {
                    true => RabbitMQOffersScraperProps.OFFERS_UPDATE_ROUTING_KEY,
                    false => RabbitMQOffersScraperProps.OFFERS_CREATE_ROUTING_KEY
                };

                _scrapperMessageProducer.SendMessage
                    (RabbitMQOffersScraperProps.OFFERS_SCRAPER_EXCHANGE, routingKey);

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }
    }
}
