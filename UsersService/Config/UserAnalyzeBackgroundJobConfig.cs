using Microsoft.Extensions.Options;
using Quartz;
using UsersService.Jobs;

namespace UsersService.Config
{
    public class UserAnalyzeBackgroundJobConfig : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(UserAnalyzeBackgroundJob));

            // Future - more difficult scheadule according to needs

            options
                .AddJob<UserAnalyzeBackgroundJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                 .AddTrigger(trigger =>
                    trigger.ForJob(jobKey)
                        .WithSimpleSchedule(scheadule =>
                        scheadule.WithIntervalInMinutes(60).RepeatForever()));
        }
    }
}
