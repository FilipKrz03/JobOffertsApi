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
    public class CheckIfOutdateEventSenderJobConfig : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(CheckIfOutdateEventSenderJob));

            options
                .AddJob<CheckIfOutdateEventSenderJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                    .AddTrigger(trigger =>
                        trigger.ForJob(jobKey)
                        .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(21, 5))
                         .Build());
        }
    }
}
