using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Jobs
{
    public class CheckIfOutdateEventSenderJob : IJob
    {
        
        private readonly IServiceProvider _serviceProvider;

        public CheckIfOutdateEventSenderJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Working !!");

            return Task.CompletedTask;
        }
    }
}
