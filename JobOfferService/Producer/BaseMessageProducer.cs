using JobOfferService.Config;
using JobOfferService.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace JobOfferService.Producer
{
    public abstract class BaseMessageProducer<T> : RabbitBaseConfig, IMessageProducer<T>
    {
        public BaseMessageProducer(string exchangeName, string routingKey, string createQueue,
            string clientProvidedName) : base(exchangeName, routingKey, createQueue, clientProvidedName) { }

        public void SendMessage(T message)
        {
            var jsonString = JsonConvert.SerializeObject(message);

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(jsonString);

            _chanel.BasicPublish(ExchangeName, RoutingKey, null, messageBodyBytes);
        }

        public void CloseConnection()
        {
            _connection.Close();
            _chanel.Close();
        }
    }
}
