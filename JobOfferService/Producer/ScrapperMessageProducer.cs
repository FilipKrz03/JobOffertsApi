using JobOfferService.Config;
using JobOfferService.Interfaces;
using JobOfferService.Props;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace JobOfferService.Producer
{
    public class ScrapperMessageProducer : RabbitBaseConfig , IScrapperMessageProducer
    {
        public ScrapperMessageProducer() : base(RabbitMQJobOffersScraperEventProps.OFFERS_SCRAPPER_CLIENT_PROVIDED_NAME)
        {
            _chanel.ExchangeDeclare(RabbitMQJobOffersScraperEventProps.OFFERS_SCRAPER_EXCHANGE, ExchangeType.Direct);

            _chanel.QueueDeclare(RabbitMQJobOffersScraperEventProps.OFFERS_CREATE_QUEUE, false, false, false);
            _chanel.QueueBind(RabbitMQJobOffersScraperEventProps.OFFERS_CREATE_QUEUE , 
                RabbitMQJobOffersScraperEventProps.OFFERS_SCRAPER_EXCHANGE , RabbitMQJobOffersScraperEventProps.OFFERS_CREATE_ROUTING_KEY);
        }


        public void SendCreateOffersMessage()
        {
            _chanel.BasicPublish(RabbitMQJobOffersScraperEventProps.OFFERS_SCRAPER_EXCHANGE,
                RabbitMQJobOffersScraperEventProps.OFFERS_CREATE_ROUTING_KEY , null , null);
        }
        
        public void CloseConnection()
        {
            _connection.Close();
            _chanel.Close();
        }
    }
}
