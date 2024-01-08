using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobOffersMapperService.Props.RabbitMqJobScrapperProps;

namespace JobOffersMapperService.Producer
{
    public class JobCheckIfOutdatedMessageProducer : RabbitBaseMessageProducer, IJobCheckIfOutdatedMessageProducer
    {
        public JobCheckIfOutdatedMessageProducer() : base(
            Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
            JOB_CHECK_IF_OUTDATED_CLIENT_PROVIDED_NAME,
            false
            )
        {
            DeclareQueueAndExchange(
                JOB_CHECK_IF_OUTDATED_QUEUE,
                JOB_SCRAPPER_EVENTS_EXCHANGE ,
                JOB_CHECK_IF_OUTDATED_ROUTING_KEY
                );
        }
    }
}
