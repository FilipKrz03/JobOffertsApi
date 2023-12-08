using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOffersApiCore.Interfaces
{
    public interface IRabbitMessageProducer
    {
        void SendMessage<T>(string exchange, string routingKey, T? message);
        void SendMessage(string exchange, string routingKey);

        void CloseConnection();
    }
}
