using JobOffersApiCore.BaseConfigurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.BaseObjects
{
    public abstract class RabbitMqAsyncBaseConsumer : RabbitBaseConfig, IHostedService
    {

        private readonly ILogger _logger;
        private readonly string _queue;

        public RabbitMqAsyncBaseConsumer(string connectionUri, string clientProvidedName,
            ILogger<RabbitMqAsyncBaseConsumer> logger, string queue)
            : base(connectionUri, clientProvidedName, true)
        {
            _logger = logger;
            _queue = queue; 
        }

        protected abstract Task ProccesMessageAsync(string message);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_chanel);

            _logger.LogInformation("Consumer started working");

            consumer.Received += async (model, ea) =>
            {
                _logger.LogInformation("New event recived");

                string body = Encoding.UTF8.GetString(ea.Body.ToArray());

                await ProccesMessageAsync(body);
            };

            _chanel.BasicConsume(_queue, true, consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            try
            {
                _chanel.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Chanel failed to stop, probably already stopper {ex}", ex);
            }

            _logger.LogWarning("Offer to crate consumer end working");

            return Task.CompletedTask;
        }
    }
}
