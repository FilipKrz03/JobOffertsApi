using JobOffersApiCore.BaseConfigurations;
using JobOffersService.Interfaces;
using JobOffersService.Props;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace JobOffersService.Consumer
{
    public class OffersToCreateConsumer : RabbitBaseConfig , IHostedService
    {

        private readonly ILogger<OffersToCreateConsumer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public OffersToCreateConsumer(ILogger<OffersToCreateConsumer> logger , 
            IServiceProvider serviceProvider):base
            (Environment.GetEnvironmentVariable("RabbitConnectionUri")!, RabbitMqJobCreateEventPros.JOB_CREATE_CLIENT_PROVIDED_NAME , true)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            _chanel.ExchangeDeclare(RabbitMqJobCreateEventPros.JOB_OFFER_EXCHANGE, ExchangeType.Direct);
            _chanel.QueueDeclare(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE, false, false, false);

            _chanel.QueueBind(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE,
                RabbitMqJobCreateEventPros.JOB_OFFER_EXCHANGE, RabbitMqJobCreateEventPros.JOB_CREATE_ROUTING_KEY);

            var consumer = new AsyncEventingBasicConsumer(_chanel);

            consumer.Received += async (model, ea) =>
            {
                string body = Encoding.UTF8.GetString(ea.Body.ToArray());

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    _logger.LogInformation("OffersToCreateConsumer recived an event");

                    IProcessedOfferService processedOfferService = 
                        scope.ServiceProvider.GetService<IProcessedOfferService>()!;

                    await processedOfferService.HandleProcessedOffer(body);
                }
            };

            _chanel.BasicConsume(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE , true , consumer);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Offert to create consumer start working");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _chanel.Close();
                _connection.Close();
            }
            catch(Exception ex)
            {
                _logger.LogWarning("Chanel failed to stop, probably already stopper {ex}", ex);
            }

            _logger.LogWarning("Offer to crate consumer end working");

            return Task.CompletedTask;
        }
    }
}
