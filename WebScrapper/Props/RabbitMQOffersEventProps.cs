using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Props
{
    internal static class RabbitMQOffersEventProps
    {
        public const string OFFERS_EVENT_CONSUMER_PROVIDED_NAME = "Offers Event Consumer";
         
        public const string OFFERS_EVENT_EXCHANGE = "offers_scrapper_events";

        public const string OFFERS_CREATE_QUEUE = "offers.create";
        public const string OFFERS_CREATE_ROUTING_KEY = "offers.create";

        public const string OFFERS_UPDATE_QUEUE = "offers.update";
        public const string OFFERS_UPDATE_ROUTING_KEY = "offers.update";
    }
}
