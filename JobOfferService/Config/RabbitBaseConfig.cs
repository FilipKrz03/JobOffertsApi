﻿using RabbitMQ.Client;

namespace JobOfferService.Config
{
    public abstract class RabbitBaseConfig
    {
        protected readonly IConnection _connection;
        protected readonly IModel _chanel;
       
        public RabbitBaseConfig(string clinetProvidedName)
        {
            string connectionUri = Environment.GetEnvironmentVariable("RabbitConnectionUri")!;

            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionUri),
                ClientProvidedName = clinetProvidedName
            };

            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
        }
    }
}
