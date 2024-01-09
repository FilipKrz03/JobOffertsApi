using JobOffersMapperService.Jobs;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersMapperService.Config
{
    public class FindOutdatedJobOffersEventSenderJobConfig : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(FindOutdatedJobOffersEventSenderJob));

            options
                .AddJob<FindOutdatedJobOffersEventSenderJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                    .AddTrigger(trigger =>
                        trigger.ForJob(jobKey)
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 38))
                         .Build());
        }
    }
}
