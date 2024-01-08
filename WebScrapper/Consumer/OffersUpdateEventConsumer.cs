using JobOffersApiCore.BaseObjects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using static WebScrapperService.Props.RabbitMQOffersEventProps;

namespace WebScrapperService.Consumer
{
    internal sealed class OffersUpdateEventConsumer : RabbitMqBaseConsumer
    {

        private readonly IServiceProvider _serviceProvider;

        public OffersUpdateEventConsumer(
            ILogger<OffersUpdateEventConsumer> logger,
            IServiceProvider serviceProvider
            )
            : base(
                 Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                 OFFERS_EVENT_CONSUMER_PROVIDED_NAME,
                 logger,
                 OFFERS_UPDATE_QUEUE
                 )
        {
            _serviceProvider = serviceProvider;

            DeclareQueueAndExchange(
                OFFERS_UPDATE_QUEUE,
                OFFERS_EVENT_EXCHANGE,
                OFFERS_UPDATE_ROUTING_KEY
                );
        }

        protected override void ProccesMessage(string message)
        {
            IServiceScope serviceScope = _serviceProvider.CreateScope();
            IOffersService offersService =
                serviceScope.ServiceProvider.GetRequiredService<IOffersService>();

            offersService.HandleOffersCreateAndUpdate(message);
        }
    }
}
