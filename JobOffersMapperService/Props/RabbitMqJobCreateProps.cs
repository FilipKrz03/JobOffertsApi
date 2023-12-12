using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Props
{
    public static class RabbitMqJobCreateProps
    {
        public const string JOB_OFFER_EXCHANGE = "job_events";
        public const string JOB_CREATE_QUEUE = "job.create";
        public const string JOB_CREATE_ROUTING_KEY = "job.create";
        public const string JOB_CREATE_CLIENT_PROVIDED_NAME = "Job Offer Create Event Sender";
    }
}
