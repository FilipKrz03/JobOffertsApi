﻿using JobOffersApiCore.BaseConfigurations;
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

namespace WebScrapperService.Producer
{
    public class JobHandleMessageProducer : RabbitMessageProducer , IRabbitMessageProducer
    {
        public JobHandleMessageProducer():
            base(Environment.GetEnvironmentVariable("RabbitConnectionUri")! , RabbitMQJobProps.JOB_CREATE_CLIENT_PROVIDED_NAME , false)
        {
            _chanel.ExchangeDeclare(RabbitMQJobProps.JOB_OFFER_EXCHANGE, ExchangeType.Direct);

            _chanel.QueueDeclare(RabbitMQJobProps.JOB_CREATE_QUEUE, false, false, false);
            _chanel.QueueBind(RabbitMQJobProps.JOB_CREATE_QUEUE, RabbitMQJobProps.JOB_OFFER_EXCHANGE,
                RabbitMQJobProps.JOB_CREATE_ROUTING_KEY, null);
        }
    }
}