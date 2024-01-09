using JobOffersApiCore.BaseConfigurations;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using static WebScrapperService.Props.RabbitMqJobDeleteProps;

namespace WebScrapperService.Producer
{
    public sealed class JobDeleteMessageProducer : RabbitBaseMessageProducer, IJobDeleteMessageProducer
    {
        public JobDeleteMessageProducer() :
            base(
                Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                JOB_DELETE_CLIENT_PROVIDED_NAME,
                false
                )
        {
            DeclareQueueAndExchange(
                JOB_DELETE_QUEUE,
                JOB_DELETE_EXCHANGE,
                JOB_DELETE_ROUTING_KEY,
                ExchangeType.Fanout
                );
        }
    }
}
