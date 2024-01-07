using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using static UsersService.Props.RabbitMqMailSendProps;

namespace UsersService.Producer
{
    public class SendEmailWithRecomendedOffersToUsersGroupMessageProducer : RabbitBaseMessageProducer , IRabbitMessageProducer
    {
        public SendEmailWithRecomendedOffersToUsersGroupMessageProducer
            () : base(Environment.GetEnvironmentVariable("RabbitConnectionUri")!, MAIL_SEND_CLIENT_PROVIDED_NAME ,  false )
        {
            DeclareQueueAndExchange(
                MAIL_SEND_QUEUE,
                MAIL_SEND_EXCHANGE,
                MAIL_SEND_ROUTING_KEY
                );
        }
    }
}
