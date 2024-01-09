using JobOffersApiCore.BaseObjects;
using JobOffersMapperService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobOffersMapperService.Props.RabbitMqJobDeleteEventProps;

namespace JobOffersMapperService.Consumer
{
    public class OffersToDeleteConsumer : RabbitMqAsyncBaseConsumer
    {

        private readonly IServiceProvider _serviceProvider;

        public OffersToDeleteConsumer(
            ILogger<OffersToDeleteConsumer> logger , 
            IServiceProvider serviceProvider
            ) :
            base(
                Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                JOB_DELETE_CLIENT_PROVIDED_NAME,
                logger,
                JOB_DELETE_QUEUE
                )
        {
            _serviceProvider = serviceProvider;

            DeclareQueueAndExchange(
                JOB_DELETE_QUEUE,
                JOB_DELETE_EXCHANGE,
                JOB_DELETE_ROUTING_KEY,
                ExchangeType.Fanout
                );
        }
        protected override async Task ProccesMessageAsync(string message)
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            IJobOfferBaseService jobOfferBaseService =
                scope.ServiceProvider.GetRequiredService<IJobOfferBaseService>();

            await jobOfferBaseService.DeleteJobOfferFromEvent(message);
        }
    }
}
