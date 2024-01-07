using JobOffersApiCore.BaseConfigurations;
using JobOffersApiCore.Interfaces;
using static UsersService.Props.RabbitMqMailSendProps;

namespace UsersService.Producer
{
    public class SendEmailWithRecomendedOffersToUsersGroupMessageProducer : RabbitBaseMessageProducer , IRabbitMessageProducer
    {
        public SendEmailWithRecomendedOffersToUsersGroupMessageProducer
            () : base(Environment.GetEnvironmentVariable("RabbitConnectionUri")!, MAIL_SENDER_CLIENT_PROVIDED_NAME ,  false )
        {
            DeclareQueueAndExchange(
                MAIL_WITH_RECOMENDED_OFFERS_TO_USER_GROUP_QUEUE,
                MAIL_SENDER_EXCHANGE,
                MAIL_WITH_RECOMENDED_OFFERS_TO_USER_GROUP_ROUTING_KEY
                );
        }
    }
}
