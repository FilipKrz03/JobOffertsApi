using JobOffersService.Jobs;
using Microsoft.Extensions.Options;
using Quartz;

namespace JobOffersService.Config
{
    public class ScrapperEventManagerJobConfig : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(ScrapperEventManagerJob));

            options
                .AddJob<ScrapperEventManagerJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                 .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(scheadule =>
                            scheadule.WithIntervalInMinutes(180).RepeatForever()));
        }
    }
}
