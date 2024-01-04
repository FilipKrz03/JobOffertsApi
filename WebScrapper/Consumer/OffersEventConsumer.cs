using JobOffersApiCore.BaseConfigurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;

namespace WebScrapperService.Consumer
{
    // Opmitalization Todo ! 
    public class OffersEventConsumer : RabbitBaseConfig , IHostedService
    {

        private readonly IOffersService _offersService;
        private readonly ILogger<OffersEventConsumer> _logger;  

        public OffersEventConsumer(IOffersService offersService  , ILogger<OffersEventConsumer> logger)
            :base(Environment.GetEnvironmentVariable("RabbitConnectionUri")!
            , RabbitMQOffersEventProps.OFFERS_EVENT_CONSUMER_PROVIDED_NAME , false)
        {
            _offersService = offersService;
            _logger = logger;

            _chanel.ExchangeDeclare(RabbitMQOffersEventProps.OFFERS_EVENT_EXCHANGE, ExchangeType.Direct);

            _chanel.QueueDeclare(RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_QUEUE_NAME, false, false, false);
            _chanel.QueueBind(RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_QUEUE_NAME, RabbitMQOffersEventProps.OFFERS_EVENT_EXCHANGE,
                RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_ROUTRING_KEY);

            _chanel.QueueDeclare(RabbitMQOffersEventProps.OFFERS_UPDATE_QUEUE , false, false, false);
            _chanel.QueueBind(RabbitMQOffersEventProps.OFFERS_UPDATE_QUEUE, RabbitMQOffersEventProps.OFFERS_EVENT_EXCHANGE,
                RabbitMQOffersEventProps.OFFERS_UPDATE_ROUTING_KEY);

            var consumer = new EventingBasicConsumer(_chanel);

            consumer.Received += (model, ea) =>
            {
                _logger.LogInformation("New event recived {message}", ea.RoutingKey);

                _offersService.HandleOffersCreateAndUpdate(ea.RoutingKey);
            };

            _chanel.BasicConsume(RabbitMQOffersEventProps.OFFERS_CREATE_EVENT_QUEUE_NAME, true, consumer);
            _chanel.BasicConsume(RabbitMQOffersEventProps.OFFERS_UPDATE_QUEUE, true, consumer);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Offers event consumer start working");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _chanel.Close();
            _connection.Close();

            _logger.LogWarning("Offers event consumer end working");

            return Task.CompletedTask;  
        }
    }
}
