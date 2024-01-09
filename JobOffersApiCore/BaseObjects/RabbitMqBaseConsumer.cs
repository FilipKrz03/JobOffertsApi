using JobOffersApiCore.BaseConfigurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace JobOffersApiCore.BaseObjects
{
    public abstract class RabbitMqBaseConsumer : RabbitBaseConfig, IHostedService
    {

        private readonly ILogger _logger;
        private readonly string _queue;

        public RabbitMqBaseConsumer(
            string connectionUri,
            string clientProvidedName,
            ILogger<RabbitMqBaseConsumer> logger,
            string queue
            ) : base(
                connectionUri,
                clientProvidedName,
                false
                )
        {
            _logger = logger;
            _queue = queue;
        }

        protected abstract void ProccesMessage(string message);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_chanel);

            _logger.LogInformation("Consumer started working");

            consumer.Received += (model, ea) =>
            {
                string body = Encoding.UTF8.GetString(ea.Body.ToArray());

                ProccesMessage(body);
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

