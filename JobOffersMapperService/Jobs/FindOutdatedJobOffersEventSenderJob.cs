using JobOffersMapperService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JobOffersMapperService.Props.RabbitMqJobScrapperProps;

namespace JobOffersMapperService.Jobs
{
    public class FindOutdatedJobOffersEventSenderJob : IJob
    {

        private readonly IServiceProvider _serviceProvider;

        public FindOutdatedJobOffersEventSenderJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            IServiceScope scope = _serviceProvider.CreateScope();

            IJobOffersBaseRepository jobOffersBaseRepository =
                scope.ServiceProvider.GetRequiredService<IJobOffersBaseRepository>();
            IJobCheckIfOutdatedMessageProducer jobCheckIfOutdatedMessageProducer =
                scope.ServiceProvider.GetRequiredService<IJobCheckIfOutdatedMessageProducer>();

            var allOffers = await jobOffersBaseRepository.GetAllJobOffersWithIdTitleLinkAsync();

            foreach (var offer in allOffers)
            {
                jobCheckIfOutdatedMessageProducer.SendMessage(
                    JOB_SCRAPPER_EVENTS_EXCHANGE ,
                    JOB_CHECK_IF_OUTDATED_ROUTING_KEY , 
                    offer
                    );
            }
        }
    }
}
