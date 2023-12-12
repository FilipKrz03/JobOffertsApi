using JobOffersApiCore.BaseConfigurations;
using JobOffersService.Props;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JobOffersService.Consumer
{
    public class OffersToCreateConsumer : RabbitBaseConfig , IHostedService
    {
        public OffersToCreateConsumer():base
            (Environment.GetEnvironmentVariable("RabbitConnectionUri")!, RabbitMqJobCreateEventPros.JOB_CREATE_CLIENT_PROVIDED_NAME , false)
        {
            _chanel.ExchangeDeclare(RabbitMqJobCreateEventPros.JOB_OFFER_EXCHANGE, ExchangeType.Direct);
            _chanel.QueueDeclare(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE, false, false, false);

            _chanel.QueueBind(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE,
                RabbitMqJobCreateEventPros.JOB_OFFER_EXCHANGE, RabbitMqJobCreateEventPros.JOB_CREATE_ROUTING_KEY);

            var consumer = new EventingBasicConsumer(_chanel);

            consumer.Received += (model, ea) =>
            {
                Console.WriteLine("Event recived");
            };

            _chanel.BasicConsume(RabbitMqJobCreateEventPros.JOB_CREATE_QUEUE , true , consumer);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Start");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _chanel.Close();
            _connection.Close();

            Console.WriteLine("End");

            return Task.CompletedTask;
        }
    }
}
