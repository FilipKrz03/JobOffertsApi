namespace UsersService.Props
{
    public static class RabbitMqMailSendProps
    {
        public const string MAIL_SENDER_EXCHANGE = "mail.send";

        public const string MAIL_WITH_RECOMENDED_OFFERS_TO_USER_GROUP_QUEUE = "mail.reomended_offers";
        public const string MAIL_WITH_RECOMENDED_OFFERS_TO_USER_GROUP_ROUTING_KEY = "mail.reomended_offers";

        public const string MAIL_SENDER_CLIENT_PROVIDED_NAME = "Mail sending event";
    }
}
