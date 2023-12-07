using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Config;
using WebScrapperService.Dto;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;

namespace WebScrapperService.Producer
{
    public class JobMessageProducer : RabbitBaseConfig , IJobOfferMessageProducer
    {
        public JobMessageProducer():base(RabbitMQJobProps.JOB_CREATE_CLIENT_PROVIDED_NAME)
        {
            _chanel.ExchangeDeclare(RabbitMQJobProps.JOB_OFFER_EXCHANGE, ExchangeType.Direct);
            _chanel.QueueDeclare(RabbitMQJobProps.JOB_CREATE_QUEUE, false, false, false);
            _chanel.QueueBind(RabbitMQJobProps.JOB_CREATE_QUEUE, RabbitMQJobProps.JOB_OFFER_EXCHANGE,
                RabbitMQJobProps.JOB_CREATE_ROUTING_KEY, null);
        }

        public void SendMessage(JobOffer mesaage)
        {
            var jsonString = JsonConvert.SerializeObject(mesaage);

            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(jsonString);

            _chanel.BasicPublish(RabbitMQJobProps.JOB_OFFER_EXCHANGE, 
                RabbitMQJobProps.JOB_CREATE_ROUTING_KEY, null, messageBodyBytes);
        }

        public void CloseConnection()
        {
            _chanel.Close();
            _connection.Close();
        }
    }
}
