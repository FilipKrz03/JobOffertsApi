using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.BaseObjects;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Props;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobOffersMapperService.Props.RabbitMqJobHandleEventProps;


namespace JobOffersMapperService.Consumer
{
    public class RawOffersConsumer : RabbitMqAsyncBaseConsumer, IHostedService
    {

        private readonly IServiceProvider _serviceProvider;

        public RawOffersConsumer(IServiceProvider serviceProvider, ILogger<RawOffersConsumer> logger)
            : base
            (Environment.GetEnvironmentVariable("RabbitConnectionUri")!, 
                  JOB_HANDLE_CLIENT_PROVIDED_NAME, logger, JOB_HANDLE_QUEUE)
        {
            _serviceProvider = serviceProvider;

            DeclareQueueAndExchange(JOB_HANDLE_QUEUE, JOB_OFFER_EXCHANGE, JOB_HANDLE_ROUTING_KEY);
        }

        protected override async Task ProccesMessageAsync(string message)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                IRawJobOfferService rawOffersService =
                    scope.ServiceProvider.GetRequiredService<IRawJobOfferService>();

                await rawOffersService.HandleRawOfferAsync(message);
            };
        }
    }
}
