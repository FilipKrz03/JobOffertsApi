namespace UsersService.Props
{
    public static class RabbitMqMailSendProps
    {
        public const string MAIL_SEND_EXCHANGE = "mail.events";

        public const string MAIL_SEND_QUEUE = "mail.send";
        public const string MAIL_SEND_ROUTING_KEY = "mail.send";

        public const string MAIL_SEND_CLIENT_PROVIDED_NAME = "Mail send producer";
    }
}
