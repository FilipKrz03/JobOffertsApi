using Quartz;
using UsersService.Interfaces.ServicesInterfaces;

namespace UsersService.Jobs
{
    public class UserAnalyzeBackgroundJob : IJob
    {

        private readonly IServiceProvider _serviceProvider;

        public UserAnalyzeBackgroundJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using IServiceScope serviceScope = _serviceProvider.CreateScope();
            IUserAnalyzeService userAnalyzeService = 
                serviceScope.ServiceProvider.GetRequiredService<IUserAnalyzeService>();

            await userAnalyzeService.LetUsersKnowAboutNewMatchingOffers();
        }
    }
}
