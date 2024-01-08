using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Props
{
    public static class RabbitMqJobScrapperProps
    {
        public const string JOB_SCRAPPER_EVENTS_EXCHANGE = "job_scrapper_events";

        public const string JOB_CHECK_IF_OUTDATED_QUEUE = "job.check_if_outdated";
        public const string JOB_CHECK_IF_OUTDATED_ROUTING_KEY = "job.check_if_outdated";
        public const string JOB_CHECK_IF_OUTDATED_CLIENT_PROVIDED_NAME = "Job Check If Outdated Event Sender";
    }
}
