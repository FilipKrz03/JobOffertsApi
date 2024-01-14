using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Props
{
    public static class RabbitMqJobHandleEventProps
    {
        public const string JOB_OFFER_EXCHANGE = "job_events";
        public const string JOB_HANDLE_QUEUE = "job_offer.handle";
        public const string JOB_HANDLE_ROUTING_KEY = "job_offer.handle";

        public const string JOB_HANDLE_CLIENT_PROVIDED_NAME = "Job Events Handler";
    }
}
