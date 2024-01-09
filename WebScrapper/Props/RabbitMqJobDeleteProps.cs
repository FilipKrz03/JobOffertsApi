using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapperService.Props
{
    public static class RabbitMqJobDeleteProps
    {
        // This props are not in RabbitMqJobProps because they need diffrent exchange 
        // They need exchange with type Fanout . Whic is diffrent from RabbitMqJobProps exchange

        public const string JOB_DELETE_EXCHANGE = "job_delete_events";
        public const string JOB_DELETE_QUEUE = "job.delete";
        public const string JOB_DELETE_ROUTING_KEY = "job.deletew";

        public const string JOB_DELETE_CLIENT_PROVIDED_NAME = "Job Delete Event Producer";
    }
}
