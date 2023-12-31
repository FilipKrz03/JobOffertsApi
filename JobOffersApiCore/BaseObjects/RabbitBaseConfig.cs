﻿using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JobOffersApiCore.BaseConfigurations
{
    public abstract class RabbitBaseConfig
    {
        protected readonly IConnection _connection;
        protected readonly IModel _chanel;

        public RabbitBaseConfig(
            string connectionUri,
            string clinetProvidedName,
            bool asyncMode
            )
        {
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionUri),
                ClientProvidedName = clinetProvidedName,
                DispatchConsumersAsync = asyncMode,
            };

            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
        }

        protected void DeclareQueueAndExchange(string queueName, string exchangeName, string routingKey)
        {
            _chanel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _chanel.QueueDeclare(queueName, false, false, false);

            _chanel.QueueBind(queueName, exchangeName, routingKey);
        }

        protected void DeclareQueueAndExchange
            (string queueName, string exchangeName, string routingKey, string exchangeType)
        {
            _chanel.ExchangeDeclare(exchangeName, exchangeType);
            _chanel.QueueDeclare(queueName, false, false, false);

            _chanel.QueueBind(queueName, exchangeName, routingKey);
        }
    }
}
