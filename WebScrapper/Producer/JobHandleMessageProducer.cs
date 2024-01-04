using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;
using static WebScrapperService.Props.RabbitMQJobProps;

namespace WebScrapperService.Producer
{
    public class JobHandleMessageProducer : RabbitBaseMessageProducer , IRabbitMessageProducer
    {
        public JobHandleMessageProducer():
            base(Environment.GetEnvironmentVariable("RabbitConnectionUri")! , RabbitMQJobProps.JOB_CREATE_CLIENT_PROVIDED_NAME , false)
        {
            DeclareQueueAndExchange(JOB_CREATE_QUEUE, JOB_OFFER_EXCHANGE, JOB_CREATE_ROUTING_KEY);
        }
    }
}
