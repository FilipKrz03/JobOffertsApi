using JobOffersApiCore.BaseObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WebScrapperService.Props.RabbitMQJobEventProps;

namespace WebScrapperService.Consumer
{
    internal sealed class OfferCheckIfOutdatedEventConsumer : RabbitMqBaseConsumer
    {
        public OfferCheckIfOutdatedEventConsumer(ILogger<OfferCheckIfOutdatedEventConsumer> logger) :
            base(
                Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                JOB_CHECK_IF_OUTDATED_CLIENT_PROVIDED_NAME,
                logger,
                JOB_CHECK_IF_OUTDATED_QUEUE
                )
        {
            DeclareQueueAndExchange(
                JOB_CHECK_IF_OUTDATED_QUEUE,
                JOB_SCRAPPER_EVENTS_EXCHANGE,
                JOB_CHECK_IF_OUTDATED_ROUTING_KEY
                );
        }

        protected override void ProccesMessage(string message)
        {
            
        }
    }
}
