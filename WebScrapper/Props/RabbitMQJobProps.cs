using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Props
{
    public static class RabbitMQJobProps
    {
        public const string JOB_OFFER_EXCHANGE = "job_events";
        public const string JOB_CREATE_QUEUE = "job.handle";
        public const string JOB_CREATE_ROUTING_KEY = "job.handle";
        public const string JOB_CREATE_CLIENT_PROVIDED_NAME = "Job Offer Event Sender Service";
    }
}
