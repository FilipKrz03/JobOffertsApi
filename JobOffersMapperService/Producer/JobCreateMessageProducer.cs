﻿using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using JobOffersMapperService.Interfaces;
using JobOffersMapperService.Props;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobOffersMapperService.Props.RabbitMqJobProps;

namespace JobOffersMapperService.Producer
{
    public class JobCreateMessageProducer : RabbitBaseMessageProducer, IJobCreateMessageProducer
    {
        public JobCreateMessageProducer() : base(
            Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
            JOB_CREATE_CLIENT_PROVIDED_NAME,
            false
            )
        {
            DeclareQueueAndExchange(
                JOB_CREATE_QUEUE,
                JOB_OFFER_EXCHANGE,
                JOB_CREATE_ROUTING_KEY
                );
        }
    }
}
