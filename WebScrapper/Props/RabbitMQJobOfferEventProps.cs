using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Props
{
    public record class RabbitMQJobOfferEventProps
    {
        public const string JOB_OFFER_EXCHANGE = "job_offer_events";
        public const string JOB_CREATE_QUEUE = "job.create";
        public const string JOB_CREATE_ROUTING_KEY = "job.create";
        public const string JOB_CREATE_CLIENT_PROVIDED_NAME = "Job Offer Event Sender Service";
    }
}
