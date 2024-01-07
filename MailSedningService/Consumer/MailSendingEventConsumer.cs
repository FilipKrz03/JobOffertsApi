using JobOffersApiCore.BaseObjects;
using MailSedningService.Dto;
using MailSedningService.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MailSedningService.Props.MailSendEventProps;

namespace MailSedningService.Consumer
{
    public class MailSendingEventConsumer : RabbitMqBaseConsumer
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MailSendingEventConsumer> _logger;

        public MailSendingEventConsumer(
            ILogger<MailSendingEventConsumer> logger,
            IServiceProvider serviceProvider
            )
            : base(
                 Environment.GetEnvironmentVariable("RabbitConnectionUri")!,
                 MAIL_SEND_CLIENT_PROVIDED_NAME,
                 logger,
                 MAIL_SEND_QUEUE
                 )
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            DeclareQueueAndExchange(
                MAIL_SEND_QUEUE,
                MAIL_SEND_EXCHANGE,
                MAIL_SEND_ROUTING_KEY
                );
        }

        protected override void ProccesMessage(string message)
        {
            var mailToSendDto = JsonConvert.DeserializeObject<MailToSendDto>(message);

            if (mailToSendDto == null )
            {
                _logger.LogWarning("Mail not parsed succesfully");
                return;
            }

            using IServiceScope scope = _serviceProvider.CreateScope();
            IMailService mailService =
                scope.ServiceProvider.GetRequiredService<IMailService>();

            mailService.SendMail(mailToSendDto);
        }
    }
}
