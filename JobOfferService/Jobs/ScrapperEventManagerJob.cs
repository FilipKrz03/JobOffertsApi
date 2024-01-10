using JobOffersApiCore.Interfaces;
using JobOffersService.Interfaces;
using Quartz;
using static JobOfferService.Props.RabbitMQOffersScraperProps;

namespace JobOffersService.Jobs
{
    public class ScrapperEventManagerJob : IJob
    {

        private readonly IRabbitMessageProducer _scrapperMessageProducer;
        private readonly IServiceProvider _serviceProvider;

        public ScrapperEventManagerJob(
            IRabbitMessageProducer scrapperMessageProducer,
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
            _scrapperMessageProducer = scrapperMessageProducer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IJobOfferRepository jobOfferRepository =
                    scope.ServiceProvider.GetService<IJobOfferRepository>()!;

                bool isDatabaseInitialized = await jobOfferRepository.IsDatabaseInitalizedAsync();

                string routingKey = isDatabaseInitialized switch
                {
                    true => OFFERS_UPDATE_ROUTING_KEY,
                    false => OFFERS_CREATE_ROUTING_KEY
                };

                string message = isDatabaseInitialized switch
                {
                    true => OFFERS_UPDATE_MESSAGE,
                    false => OFFERS_CREATE_MESSAGE
                };

                _scrapperMessageProducer.SendMessage(
                    OFFERS_SCRAPER_EXCHANGE,
                    routingKey,
                    message
                    );
            }
        }
    }
}
