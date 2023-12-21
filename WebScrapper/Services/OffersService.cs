using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;

namespace WebScrapperService.Services
{
    public class OffersService : IOffersService 
    {
        private readonly IEnumerable<IScrapperService> _scrapperServices;

        public OffersService(IEnumerable<IScrapperService> scrapperServices)
        {
            _scrapperServices = scrapperServices;
        }

        public void HandleOffersCreateAndUpdate(string routingKey)
        {
            bool isInit = routingKey switch
            {
                RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_ROUTRING_KEY => true,
                RabbitMQOffersEventProps.OFFERS_UPDATE_ROUTING_KEY => false,
                _ => true,
            };
            foreach(var service in _scrapperServices)
            {
                service.ScrapOfferts(isInit);
            }
        }
    }
}
