using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using JobOfferService.Props;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace JobOfferService.Producer
{
    public class ScrapperMessageProducer : RabbitMessageProducer , IRabbitMessageProducer
    {
        public ScrapperMessageProducer() : base(Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
            RabbitMQOffersScraperEventProps.OFFERS_SCRAPPER_CLIENT_PROVIDED_NAME)
        {
            _chanel.ExchangeDeclare(RabbitMQOffersScraperEventProps.OFFERS_SCRAPER_EXCHANGE, ExchangeType.Direct);

            _chanel.QueueDeclare(RabbitMQOffersScraperEventProps.OFFERS_CREATE_QUEUE, false, false, false);
            _chanel.QueueBind(RabbitMQOffersScraperEventProps.OFFERS_CREATE_QUEUE , 
                RabbitMQOffersScraperEventProps.OFFERS_SCRAPER_EXCHANGE , RabbitMQOffersScraperEventProps.OFFERS_CREATE_ROUTING_KEY);
        }
    }
}
