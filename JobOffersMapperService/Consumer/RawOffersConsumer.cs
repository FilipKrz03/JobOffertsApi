using JobOffersApiCore.BaseConfigurations;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Props;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JobOffersMapperService.Consumer
{
    public class RawOffersConsumer : RabbitBaseConfig , IHostedService
    {

        private readonly IRawOfferService _rawOffersService;
        private readonly ILogger<RawOffersConsumer> _logger;    

        public RawOffersConsumer(IRawOfferService rawOfferService , ILogger<RawOffersConsumer> logger)
            :base(Environment.GetEnvironmentVariable("RabbitConnectionUri")! , RabbitMqJobHandleEventProps.JOB_HANDLE_CLIENT_PROVIDED_NAME)
        {
            _rawOffersService = rawOfferService;
            _logger = logger;

            _chanel.ExchangeDeclare(RabbitMqJobHandleEventProps.JOB_OFFER_EXCHANGE, ExchangeType.Direct);
            _chanel.QueueDeclare(RabbitMqJobHandleEventProps.JOB_HANDLE_QUEUE, false, false, false);
            _chanel.QueueBind(RabbitMqJobHandleEventProps.JOB_HANDLE_QUEUE, RabbitMqJobHandleEventProps.JOB_OFFER_EXCHANGE,
                RabbitMqJobHandleEventProps.JOB_HANDLE_ROUTING_KEY);

            var consumer = new AsyncEventingBasicConsumer(_chanel);

            consumer.Received += async (model, ea) =>
            {
                _logger.LogInformation("New event recived - Raw offers consumer");

                string body = Encoding.UTF8.GetString(ea.Body.ToArray());

                await _rawOffersService.HandleRawOffer(body);
            };

            _chanel.BasicConsume(RabbitMqJobHandleEventProps.JOB_HANDLE_QUEUE, true, consumer);

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Raw offers consumer start working");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            _chanel.Close();

            _logger.LogWarning("Raw offers conusmer end working");

            return Task.CompletedTask;
        }
    }
}
