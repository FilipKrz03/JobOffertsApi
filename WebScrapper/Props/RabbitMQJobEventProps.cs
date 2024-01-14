using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Props
{
    public static class RabbitMQJobEventProps
    {
        public const string JOB_SCRAPPER_EVENTS_EXCHANGE = "job_scrapper_events";

        public const string JOB_CHECK_IF_OUTDATED_QUEUE = "job_offer.check_if_outdated";
        public const string JOB_CHECK_IF_OUTDATED_ROUTING_KEY = "job_offer.check_if_outdated";
        public const string JOB_CHECK_IF_OUTDATED_CLIENT_PROVIDED_NAME = "Job Check If Outdated Event Consumer";
    }
}
