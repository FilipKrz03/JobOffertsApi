using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using JobOfferService.Props;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using static JobOfferService.Props.RabbitMQOffersScraperProps;

namespace JobOfferService.Producer
{
    public class ScrapperMessageProducer : RabbitBaseMessageProducer, IRabbitMessageProducer
    {
        public ScrapperMessageProducer() :
            base(
                Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                OFFERS_SCRAPPER_CLIENT_PROVIDED_NAME,
                false
                )
        {
            DeclareQueueAndExchange(
                OFFERS_CREATE_QUEUE,
                OFFERS_SCRAPER_EXCHANGE,
                OFFERS_CREATE_ROUTING_KEY
                );

            DeclareQueueAndExchange(
                OFFERS_UPDATE_QUEUE,
                OFFERS_SCRAPER_EXCHANGE,
                OFFERS_UPDATE_ROUTING_KEY
                );
        }
    }
}
