using JobOffersApiCore.BaseObjects;
using JobOffersApiCore.Dto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using static WebScrapperService.Props.RabbitMQJobEventProps;

namespace WebScrapperService.Consumer
{
    public sealed class OfferCheckIfOutdatedEventConsumer : RabbitMqBaseConsumer
    {

        private readonly IServiceProvider _serviceProvider;

        public OfferCheckIfOutdatedEventConsumer(
            ILogger<OfferCheckIfOutdatedEventConsumer> logger,
            IServiceProvider serviceProvider
            ) :
            base(
                Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                JOB_CHECK_IF_OUTDATED_CLIENT_PROVIDED_NAME,
                logger,
                JOB_CHECK_IF_OUTDATED_QUEUE
                )
        {
            _serviceProvider = serviceProvider;

            DeclareQueueAndExchange(
                JOB_CHECK_IF_OUTDATED_QUEUE,
                JOB_SCRAPPER_EVENTS_EXCHANGE,
                JOB_CHECK_IF_OUTDATED_ROUTING_KEY
                );
        }

        protected override void ProccesMessage(string message)
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            IJobTopicalityCheckerService jobTopicalityCheckerService =
                scope.ServiceProvider.GetRequiredService<IJobTopicalityCheckerService>();

            jobTopicalityCheckerService.CheckIfOfferStillExist(message);
        }
    }
}
