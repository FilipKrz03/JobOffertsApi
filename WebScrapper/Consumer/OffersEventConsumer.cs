using JobOffersApiCore.BaseConfigurations;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Props;

namespace WebScrapperService.Consumer
{
    public class OffersEventConsumer : RabbitBaseConfig , IHostedService
    {
        public OffersEventConsumer():base(Environment.GetEnvironmentVariable("RabbitConnectionUri")!
            , RabbitMQOffersEventProps.OFFERS_EVENT_CONSUMER_PROVIDED_NAME)
        {
            _chanel.ExchangeDeclare(RabbitMQOffersEventProps.OFFERS_EVENT_EXCHANGE, ExchangeType.Direct);
            _chanel.QueueDeclare(RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_QUEUE_NAME, false, false, false);
            _chanel.QueueBind(RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_QUEUE_NAME, RabbitMQOffersEventProps.OFFERS_EVENT_EXCHANGE,
                RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_ROUTRING_KEY);

            var consumer = new EventingBasicConsumer(_chanel);

            consumer.Received += (model, ea) =>
            {
                Console.WriteLine($"Message reciver : queue : {ea.RoutingKey}");
            };

            _chanel.BasicConsume(RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_QUEUE_NAME, true, consumer);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Conusmer started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _chanel.Close();
            _connection.Close();
            Console.WriteLine("Conusmer stopper");
            return Task.CompletedTask;  
        }
    }
}
