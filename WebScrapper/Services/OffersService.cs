using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;
using static WebScrapperService.Props.RabbitMQOffersEventProps;

namespace WebScrapperService.Services
{
    internal class OffersService : IOffersService 
    {
        private readonly IEnumerable<IScrapperService> _scrapperServices;
        private readonly ILogger<OffersService> _logger;

        public OffersService(IEnumerable<IScrapperService> scrapperServices , ILogger<OffersService> logger)
        {
            _scrapperServices = scrapperServices;
            _logger = logger;
        }

        public void HandleOffersCreateAndUpdate(string routingKey)
        {
            try
            {
                bool isInit = routingKey switch
                {
                    OFFERS_CREATE_ROUTING_KEY => true,
                    OFFERS_UPDATE_ROUTING_KEY => false,
                    _ => throw new ArgumentException("Not recognized routing key"),
                };

                foreach (var service in _scrapperServices)
                {
                    service.ScrapOfferts(isInit);
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("WebScrapper - Offers service error occured {ex}", ex);
            }
        }
    }
}
