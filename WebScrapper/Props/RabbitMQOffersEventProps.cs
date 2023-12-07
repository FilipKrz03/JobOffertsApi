using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Props
{
    public static class RabbitMQOffersEventProps
    {
        public const string OFFERS_EVENT_CONSUMER_PROVIDED_NAME = "Offers Event Consumer";
         
        public const string OFFERS_EVENT_ECHANGE = "offers_scrapper_events";

        public const string OFFERS_CREATE_EVENT_QUEUE_NAME = "offers.create";
        public const string OFFERS_CREATE_EVENT_ROUTRING_KEY = "offers.create";
    }
}
