﻿using JobOffersApiCore.Interfaces;
using JobOfferService.Props;
using JobOffersService.Interfaces;
using JobOffersService.Services;
using static JobOfferService.Props.RabbitMQOffersScraperProps;

namespace JobOfferService.Services
{
    public class ScrapperEventManagerService : BackgroundService
    {

        private readonly IRabbitMessageProducer _scrapperMessageProducer;
        private readonly IServiceProvider _serviceProvider;

        public ScrapperEventManagerService(
            IRabbitMessageProducer scrapperMessageProducer,
            IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
            _scrapperMessageProducer = scrapperMessageProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
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

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }
    }
}
