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
            RabbitMQOffersScraperProps.OFFERS_SCRAPPER_CLIENT_PROVIDED_NAME, false)
        {
            _chanel.ExchangeDeclare(RabbitMQOffersScraperProps.OFFERS_SCRAPER_EXCHANGE, ExchangeType.Direct);

            _chanel.QueueDeclare(RabbitMQOffersScraperProps.OFFERS_CREATE_QUEUE, false, false, false);
            _chanel.QueueBind(RabbitMQOffersScraperProps.OFFERS_CREATE_QUEUE , 
                RabbitMQOffersScraperProps.OFFERS_SCRAPER_EXCHANGE , RabbitMQOffersScraperProps.OFFERS_CREATE_ROUTING_KEY);

            _chanel.QueueDeclare(RabbitMQOffersScraperProps.OFFERS_UPDATE_QUEUE , false, false, false);
            _chanel.QueueBind(RabbitMQOffersScraperProps.OFFERS_UPDATE_QUEUE,
                RabbitMQOffersScraperProps.OFFERS_SCRAPER_EXCHANGE, RabbitMQOffersScraperProps.OFFERS_UPDATE_ROUTING_KEY);
        }
    }
}
