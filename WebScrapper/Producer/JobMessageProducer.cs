using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapperService.Dto;
using WebScrapperService.Interfaces;
using WebScrapperService.Props;

namespace WebScrapperService.Producer
{
    public class JobOfferMessageProducer : BaseMessageProducer<JobOffer> , IMessageProducer<JobOffer>
    {
        public JobOfferMessageProducer():base(RabbitMQJobOfferEventProps.JOB_OFFER_EXCHANGE , 
            RabbitMQJobOfferEventProps.JOB_CREATE_ROUTING_KEY , RabbitMQJobOfferEventProps.JOB_CREATE_QUEUE ,
            RabbitMQJobOfferEventProps.JOB_CREATE_CLIENT_PROVIDED_NAME) { }       
    }
}
