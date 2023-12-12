using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.Props;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Producer
{
    public class JobCreateMessageProducer : RabbitMessageProducer, IRabbitMessageProducer
    {
        public JobCreateMessageProducer() : base(Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
            RabbitMqJobCreateProps.JOB_CREATE_CLIENT_PROVIDED_NAME , false)
        {
            _chanel.ExchangeDeclare(RabbitMqJobCreateProps.JOB_OFFER_EXCHANGE, ExchangeType.Direct);

            _chanel.QueueDeclare(RabbitMqJobCreateProps.JOB_CREATE_QUEUE , false , false , false);
            _chanel.QueueBind(RabbitMqJobCreateProps.JOB_CREATE_QUEUE, RabbitMqJobCreateProps.JOB_OFFER_EXCHANGE,
                RabbitMqJobCreateProps.JOB_CREATE_ROUTING_KEY , null);
        }
    }
}
