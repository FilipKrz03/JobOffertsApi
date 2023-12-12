using JobOffersApiCore.BaseConfigurations;
using JobOffersService.Props;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JobOffersService.Consumer
{
    public class OffersToCreateConsumer : RabbitBaseConfig , IHostedService
    {

        private readonly ILogger<OffersToCreateConsumer> _logger;
        
        public OffersToCreateConsumer(ILogger<OffersToCreateConsumer> logger):base
            (Environment.GetEnvironmentVariable("RabbitConnectionUri")!, RabbitMqJobCreateEventPros.JOB_CREATE_CLIENT_PROVIDED_NAME , false)
        {
            _logger = logger;

            _chanel.ExchangeDeclare(RabbitMqJobCreateEventPros.JOB_OFFER_EXCHANGE, ExchangeType.Direct);
            _chanel.QueueDeclare(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE, false, false, false);

            _chanel.QueueBind(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE,
                RabbitMqJobCreateEventPros.JOB_OFFER_EXCHANGE, RabbitMqJobCreateEventPros.JOB_CREATE_ROUTING_KEY);

            var consumer = new EventingBasicConsumer(_chanel);

            consumer.Received += (model, ea) =>
            {
                _logger.LogInformation("Offers to create consumer recived event");
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
            _chanel.Close();
            _connection.Close();

            _logger.LogWarning("Offer to crate consumer end working");

            return Task.CompletedTask;
        }
    }
}
