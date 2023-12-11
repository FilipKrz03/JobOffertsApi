using JobOffersApiCore.BaseConfigurations;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Props;
using Microsoft.Extensions.Hosting;
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

        public RawOffersConsumer(IRawOfferService rawOfferService)
            :base(Environment.GetEnvironmentVariable("RabbitConnectionUri")! , RabbitMqJobHandleEventProps.JOB_HANDLE_CLIENT_PROVIDED_NAME)
        {
            _rawOffersService = rawOfferService;

            _chanel.ExchangeDeclare(RabbitMqJobHandleEventProps.JOB_OFFER_EXCHANGE, ExchangeType.Direct);
            _chanel.QueueDeclare(RabbitMqJobHandleEventProps.JOB_HANDLE_QUEUE, false, false, false);
            _chanel.QueueBind(RabbitMqJobHandleEventProps.JOB_HANDLE_QUEUE, RabbitMqJobHandleEventProps.JOB_OFFER_EXCHANGE,
                RabbitMqJobHandleEventProps.JOB_HANDLE_ROUTING_KEY);

            var consumer = new AsyncEventingBasicConsumer(_chanel);

            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine($"New event : {ea.RoutingKey}");

                string body = Encoding.UTF8.GetString(ea.Body.ToArray());

                await _rawOffersService.HandleRawOffer(body);
            };

            _chanel.BasicConsume(RabbitMqJobHandleEventProps.JOB_HANDLE_QUEUE, true, consumer);

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Consumer started work");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Close();
            _chanel.Close();
            Console.WriteLine("Consumer end work");
            return Task.CompletedTask;
        }
    }
}
