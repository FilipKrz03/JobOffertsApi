using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Config
{
    public abstract class RabbitBaseConfig
    {
        protected readonly IConnection _connection;
        protected readonly IModel _chanel;
        protected readonly string ExchangeName;
        protected readonly string RoutingKey;
        protected readonly string QueueName;

        public RabbitBaseConfig(string exchangeName, string routingKey,
            string queueName, string clinetProvidedName)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            QueueName = queueName;

            string connectionUri = Environment.GetEnvironmentVariable("RabbitConnectionUri")!;

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionUri),
                ClientProvidedName = clinetProvidedName
            };

            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();

            _chanel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _chanel.QueueDeclare(QueueName, false, false, false);
            _chanel.QueueBind(QueueName, ExchangeName, RoutingKey, null);
        }
    }
}
