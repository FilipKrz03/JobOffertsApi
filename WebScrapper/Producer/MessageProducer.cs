using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;

namespace WebScrapperService.Producer
{
    public class MessageProducer : IMessageProducer
    {
        private  IConnection _connection;
        private  IModel _chanel;
        private const string exchangeName = "OfferExchange";
        private const string routingKey = "demo-routing-key";
        private const string quequeName = "OfferQueue";

        public MessageProducer()
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672"),
                ClientProvidedName = "Rabit Scrapper Sender"
            };

            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();

            _chanel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _chanel.QueueDeclare(quequeName, false, false, false);
            _chanel.QueueBind(quequeName, exchangeName, routingKey, null);
        }

        public void SendMessage<T>(T message)
        {
            var jsonString = JsonConvert.SerializeObject(message);

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(jsonString);

            _chanel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);

        }

        public void CloseConnection()
        {
            _connection.Close();
            _chanel.Close();
        }

    }
}
