using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSedningService.Props
{
    public static class MailSendEventProps
    {
        public const string MAIL_SEND_EXCHANGE = "mail.events";

        public const string MAIL_SEND_QUEUE = "mail.send";
        public const string MAIL_SEND_ROUTING_KEY = "mail.send";

        public const string MAIL_SEND_CLIENT_PROVIDED_NAME = "Mail send consumer";
    }
}
