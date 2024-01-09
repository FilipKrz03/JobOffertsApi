using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.BaseObjects;
using JobOffersService.Interfaces;
using JobOffersService.Props;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;
using static JobOffersService.Props.RabbitMqJobCreateEventPros;

namespace JobOffersService.Consumer
{
    public class OffersToCreateConsumer : RabbitMqAsyncBaseConsumer
    {

        private readonly IServiceProvider _serviceProvider;

        public OffersToCreateConsumer(ILogger<OffersToCreateConsumer> logger, IServiceProvider serviceProvider)
            : base(Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                  JOB_CREATE_CLIENT_PROVIDED_NAME, logger, JOB_CREATE_QUEUE)
        {
            _serviceProvider = serviceProvider;

            DeclareQueueAndExchange(
                JOB_CREATE_QUEUE,
                JOB_OFFER_EXCHANGE,
                JOB_CREATE_ROUTING_KEY
                );
        }

        protected override async Task ProccesMessageAsync(string message)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IProcessedOfferService processedOfferService =
                scope.ServiceProvider.GetService<IProcessedOfferService>()!;

            await processedOfferService.HandleProcessedOffer(message);
        }
    }
}
