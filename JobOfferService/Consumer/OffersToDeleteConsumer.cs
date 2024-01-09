using JobOffersApiCore.BaseObjects;
using JobOffersService.Interfaces;
using RabbitMQ.Client;
using static JobOffersService.Props.RabbitMqJobDeleteEventProps;

namespace JobOffersService.Consumer
{
    public class OffersToDeleteConsumer : RabbitMqAsyncBaseConsumer
    {

        private readonly IServiceProvider _serviceProvider;

        public OffersToDeleteConsumer(
            ILogger<OffersToDeleteConsumer> logger,
            IServiceProvider serviceProvider
            ) :
            base(
                Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                JOB_DELETE_CLIENT_PROVIDED_NAME,
                logger,
                JOB_DELETE_QUEUE
                )
        {
            _serviceProvider = serviceProvider;

            DeclareQueueAndExchange(
                JOB_DELETE_QUEUE,
                JOB_DELETE_EXCHANGE,
                JOB_DELETE_ROUTING_KEY,
                ExchangeType.Fanout
                );
        }

        protected override async Task ProccesMessageAsync(string message)
        {
            IServiceScope scope = _serviceProvider.CreateScope();
            IJobOfferService jobOfferService = 
                scope.ServiceProvider.GetRequiredService<IJobOfferService>();

            await jobOfferService.DeleteJobOfferFromEvent(message);
        }
    }
}
